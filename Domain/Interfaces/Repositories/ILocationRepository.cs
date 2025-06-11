using Domain.DTO.Location;
using Domain.Entities.LocationReleated;

namespace Domain.Interfaces.Repositories
{
    public interface ILocationRepository :IBaseRepository<Location>
    {
        Task<List<CountryDTO>> GetCountries();
        Task<List<StateDTO>> GetSatesByCountry(int countryID);
        Task<List<CityDTO>> GetCitiesByState(int stateID);
    }
}
