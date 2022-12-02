using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoronaTracker.Services
{
    public interface IInfectionService
    {
        Task<bool> Download();
        Task<ICollection<Models.Infection>> List();
    }
}
