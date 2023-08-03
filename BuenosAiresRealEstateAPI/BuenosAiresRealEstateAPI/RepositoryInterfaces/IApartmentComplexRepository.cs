using BuenosAiresRealEstate.API.Models.Models;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.RepositoryInterfaces
{
    public interface IApartmentComplexRepository : IRepository<ApartmentComplex>
    {
        // we put the update method here and not in the superclass
        // because update implementations tend to be custom to the models
        Task<ApartmentComplex> UpdateAsync(ApartmentComplex entity);
    }
}
