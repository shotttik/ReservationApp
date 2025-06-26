using Application.Common.Requests.Company;
using Application.Common.Results;
using Domain.DTO.Company;

namespace Application.Interfaces
{
    public interface ICompanyFAQService
    {
        Task<Result> Create(int routeCompanyId, CompanyFAQCreateRequest request);
        Task<Result> Update(int routeCompanyId, int routeCategoryId, CompanyFAQUpdateRequest request);
        Task<Result> Delete(int routeCompanyId, int routeCategoryId, int id);
        Task<Result<IEnumerable<CompanyFAQDTO>>> GetAll(int companyId, int? categoryID);
    }
}
