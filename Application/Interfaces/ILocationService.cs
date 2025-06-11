using Application.Common.Results;
using Domain.DTO.Location;

namespace Application.Interfaces
{
    public interface ILocationService
    {
        Task<Result<List<CityDTO>>> GetCitiesByState(int stateID);
        Task<Result<List<StateDTO>>> GetStatesByCountry(int countryID);
        Task<Result<List<CountryDTO>>> GetCountries();
    }
}
