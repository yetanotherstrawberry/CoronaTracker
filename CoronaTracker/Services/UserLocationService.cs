using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CoronaTracker.Services
{
    public class UserLocationService : ILocationService
    {

        private static SQLiteAsyncConnection Database;

        public UserLocationService()
            => Database = new SQLiteAsyncConnection(Constants.DatabaseFile, Constants.DatabaseFlags);

        public static readonly AsyncLazy<UserLocationService> Instance = new AsyncLazy<UserLocationService>(async () =>
        {
            var instance = new UserLocationService();
            await Database.CreateTableAsync<Location>();
            return instance;
        });

        public async Task<Location> CheckLocation(bool refresh = false)
        {
            try
            {
                var location = refresh ? await Geolocation.GetLocationAsync() : await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    location.Timestamp += new DateTimeOffset(DateTime.Now).Offset;

                    var result = true;

                    if ((await Database.Table<Location>().CountAsync(x => x.Timestamp == location.Timestamp)) == 0)
                        result = await Database.InsertAsync(location) > 0;

                    return !result ? throw new Exception("CheckLocation(): result == false") : location;
                }
                else
                    return (await LocationList()).LastOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<ICollection<Location>> LocationList() => await Database.Table<Location>().ToListAsync();

        public async Task<bool> RemoveData(Location chosen = null)
            => chosen != null ? await Database.Table<Location>().DeleteAsync(x => x.Timestamp == chosen.Timestamp) > 0 : await Database.Table<Location>().DeleteAsync() > 0;

    }
}
