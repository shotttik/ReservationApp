using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO.Location;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class LocationService :ILocationService
    {
        private readonly ILocationRepository locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }
        public async Task<Result<List<CountryDTO>>> GetCountries()
        {
            var countries = await locationRepository.GetCountries();
            if (countries is null || countries.Count == 0)
            {
                return Result.Failure<List<CountryDTO>>(GenericResults.NotFound);
            }
            return Result.Success(countries, GenericResults.Success);
        }

        public async Task<Result<List<CityDTO>>> GetCitiesByState(int stateID)
        {
            var cities = await locationRepository.GetCitiesByState(stateID);
            if (cities is null || cities.Count == 0)
            {
                return Result.Failure<List<CityDTO>>(GenericResults.NotFound);
            }
            return Result.Success(cities, GenericResults.Success);
        }

        public async Task<Result<List<StateDTO>>> GetStatesByCountry(int countryID)
        {
            var states = await locationRepository.GetSatesByCountry(countryID);
            if (states is null || states.Count == 0)
            {
                return Result.Failure<List<StateDTO>>(GenericResults.NotFound);
            }
            return Result.Success(states, GenericResults.Success);
        }

    }
}
