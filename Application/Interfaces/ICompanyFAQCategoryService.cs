using Application.Common.Requests.Company;
using Application.Common.Results;
using Domain.DTO.Company;

namespace Application.Interfaces
{
    public interface ICompanyFAQCategoryService
    {
        Task<Result> Create(CompanyFAQCategoryCreateRequest request);
        Task<Result> Update(CompanyFAQCategoryUpdateRequest request);
        Task<Result> Delete(int id);
        Task<Result<IEnumerable<CompanyFAQCategoryDTO>>> GetAll(int companyID);
    }
}
