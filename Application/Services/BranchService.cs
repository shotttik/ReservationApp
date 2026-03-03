using Application.Common.Requests.Admin;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO.Branch;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class BranchService :IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IAccessGuard _accessGuard;

        public BranchService(
            IBranchRepository branchRepository,
            IAccessGuard accessGuard
        )
        {
            _branchRepository = branchRepository;
            _accessGuard = accessGuard;
        }
        public async Task<Result<List<CountryDTO>>> GetCountries()
        {
            var countries = await _branchRepository.GetCountries();
            if (countries is null || countries.Count == 0)
            {
                return Result.Failure<List<CountryDTO>>(GenericResults.NotFound);
            }
            return Result.Success(countries, GenericResults.Success);
        }

        public async Task<Result<List<CityDTO>>> GetCitiesByState(int stateID)
        {
            var cities = await _branchRepository.GetCitiesByState(stateID);
            if (cities is null || cities.Count == 0)
            {
                return Result.Failure<List<CityDTO>>(GenericResults.NotFound);
            }
            return Result.Success(cities, GenericResults.Success);
        }

        public async Task<Result<List<StateDTO>>> GetStatesByCountry(int countryID)
        {
            var states = await _branchRepository.GetSatesByCountry(countryID);
            if (states is null || states.Count == 0)
            {
                return Result.Failure<List<StateDTO>>(GenericResults.NotFound);
            }
            return Result.Success(states, GenericResults.Success);
        }
        public async Task<Result> Delete(int companyId, int branchId, bool force = false)
        {
            var accessError = await _accessGuard.EnsureAccessToCompany(companyId);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }

            var branch = await _branchRepository.Get(branchId, companyId);
            if (branch is null)
            {
                return Result.Failure(BranchResults.NotFound);
            }

            if (branch.IsDisabled)
            {
                return Result.Failure(BranchResults.AlreadyDisabled);
            }

            if (force)
            {
                await _branchRepository.Delete(branch.Id);
            }
            else
            {
                branch.Disable();
                await _branchRepository.Update(branch);
            }

            return Result.Success(BranchResults.Deleted);
        }
        public async Task<Result<BranchDTO>> Create(int companyId, BranchCreateRequest request)
        {
            var accessError = await _accessGuard.EnsureAccessToCompany(companyId);
            if (accessError != Error.None)
            {
                return Result.Failure<BranchDTO>(accessError);
            }
            var branch = request.MapToEntity();
            branch.CompanyId = companyId;
            await _branchRepository.Add(branch);

            return Result.Success(branch.MapToDTO());
        }

        public async Task<Result<BranchDTO>> Update(int companyId, int branchId, BranchUpdateRequest request)
        {
            var accessError = await _accessGuard.EnsureAccessToCompany(companyId);
            if (accessError != Error.None)
            {
                return Result.Failure<BranchDTO>(accessError);
            }

            var existingBranch = await _branchRepository.Get(branchId, companyId);
            if (existingBranch is null)
            {
                return Result.Failure<BranchDTO>(BranchResults.NotFound);
            }
            var branch = request.MapToEntity(existingBranch);
            await _branchRepository.Update(branch);

            return Result.Success(branch.MapToDTO());
        }
    }
}
