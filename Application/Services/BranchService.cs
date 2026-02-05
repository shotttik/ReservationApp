using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO.Branch;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class BranchService :IBranchService
    {
        private readonly IBranchRepository branchRepository;

        public BranchService(IBranchRepository branchRepository)
        {
            this.branchRepository = branchRepository;
        }
        public async Task<Result<List<CountryDTO>>> GetCountries()
        {
            var countries = await branchRepository.GetCountries();
            if (countries is null || countries.Count == 0)
            {
                return Result.Failure<List<CountryDTO>>(GenericResults.NotFound);
            }
            return Result.Success(countries, GenericResults.Success);
        }

        public async Task<Result<List<CityDTO>>> GetCitiesByState(int stateID)
        {
            var cities = await branchRepository.GetCitiesByState(stateID);
            if (cities is null || cities.Count == 0)
            {
                return Result.Failure<List<CityDTO>>(GenericResults.NotFound);
            }
            return Result.Success(cities, GenericResults.Success);
        }

        public async Task<Result<List<StateDTO>>> GetStatesByCountry(int countryID)
        {
            var states = await branchRepository.GetSatesByCountry(countryID);
            if (states is null || states.Count == 0)
            {
                return Result.Failure<List<StateDTO>>(GenericResults.NotFound);
            }
            return Result.Success(states, GenericResults.Success);
        }

    }
}
