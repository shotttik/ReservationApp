using Application.Common.Requests.Admin;
using Application.Common.Results;
using Domain.DTO.Branch;

namespace Application.Interfaces
{
    public interface IBranchService
    {
        Task<Result<List<CityDTO>>> GetCitiesByState(int stateID);
        Task<Result<List<StateDTO>>> GetStatesByCountry(int countryID);
        Task<Result<List<CountryDTO>>> GetCountries();
        Task<Result> Delete(int companyId, int branchId, bool force);
        Task<Result<BranchDTO>> Create(int companyId, BranchCreateRequest request);
        Task<Result<BranchDTO>> Update(int companyId, int branchId, BranchUpdateRequest request);
    }
}
