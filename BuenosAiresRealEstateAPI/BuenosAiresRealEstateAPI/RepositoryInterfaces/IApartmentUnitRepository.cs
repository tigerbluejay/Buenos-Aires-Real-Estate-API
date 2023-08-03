using BuenosAiresRealEstate.API.Models.Models;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.RepositoryInterfaces
{
    public interface IApartmentUnitRepository : IRepository<ApartmentUnit>
    {
        Task<ApartmentUnit> UpdateAsync(ApartmentUnit entity);
    }
}
