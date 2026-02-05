using Application.Common.Results;
using Domain.DTO.Branch;

namespace Application.Interfaces
{
    public interface IBranchService
    {
        Task<Result<List<CityDTO>>> GetCitiesByState(int stateID);
        Task<Result<List<StateDTO>>> GetStatesByCountry(int countryID);
        Task<Result<List<CountryDTO>>> GetCountries();
    }
}
