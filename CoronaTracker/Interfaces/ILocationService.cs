using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CoronaTracker.Services
{
    public interface ILocationService
    {
        Task<Location> CheckLocation(bool forceRefresh = false);
        Task<ICollection<Location>> LocationList();
        Task<bool> RemoveData(Location location = null);
    }
}
