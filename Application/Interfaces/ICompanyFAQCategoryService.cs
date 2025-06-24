using Application.Common.Requests.Company;
using Application.Common.Results;
using Domain.DTO.Company;

namespace Application.Interfaces
{
    public interface ICompanyFAQCategoryService
    {
        Task<Result> Create(int routeCompanyId,CompanyFAQCategoryCreateRequest request);
        Task<Result> Update(int routeCompanyId, CompanyFAQCategoryUpdateRequest request);
        Task<Result> Delete(int routeCompanyId, int id);
        Task<Result<IEnumerable<CompanyFAQCategoryDTO>>> GetAll(int companyID);
    }
}
