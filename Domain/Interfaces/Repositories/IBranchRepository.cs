using Domain.DTO.Branch;
using Domain.Entities.BranchReleated;

namespace Domain.Interfaces.Repositories
{
    public interface IBranchRepository :IBaseRepository<Branch>
    {
        Task<List<CountryDTO>> GetCountries();
        Task<List<StateDTO>> GetSatesByCountry(int countryID);
        Task<List<CityDTO>> GetCitiesByState(int stateID);
        Task<Branch?> Get(int id, int companyId);
        Task Delete(int id);
    }
}
