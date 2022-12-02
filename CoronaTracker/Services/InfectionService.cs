using CoronaTracker.Models;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoronaTracker.Services
{
    public class InfectionService : IInfectionService
    {

        private readonly HttpClient WebClient;
        private static SQLiteAsyncConnection Database;

        public InfectionService()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabaseFile, Constants.DatabaseFlags);
            WebClient = new HttpClient();
        }

        public static readonly AsyncLazy<InfectionService> Instance = new AsyncLazy<InfectionService>(async () =>
        {
            var instance = new InfectionService();
            await Database.CreateTableAsync<Infection>();
            return instance;
        });

        public async Task<ICollection<Infection>> List()
        {
            await Download();
            return await Database.Table<Infection>().ToListAsync();
        }

        public async Task<bool> Download()
        {
            try
            {
                var endpoint = string.Format(AppResources._API_GET_ENDPOINT, AppResources._API_ADDRESS, DateTime.Now.ToString());
                var response = await WebClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    ICollection<Infection> temp = JsonConvert.DeserializeObject<List<Infection>>(await response.Content.ReadAsStringAsync());

                    if (temp.Count > 0)
                    {
                        await Database.Table<Infection>().DeleteAsync(x => true); // Delete everything.
                        await Database.InsertAllAsync(temp);

                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return false;
        }

    }
}
