using Application.Common.Requests.Company;
using Application.Common.Results;
using Domain.DTO.Company;

namespace Application.Interfaces
{
    public interface ICompanyFAQService
    {
        Task<Result> Create(CompanyFAQCreateRequest request);
        Task<Result> Update(CompanyFAQUpdateRequest request);
        Task<Result> Delete(int id);
        Task<Result<CompanyFAQDTO>> Get(int id);
        Task<Result<IEnumerable<CompanyFAQDTO>>> GetAll(int categoryID);
    }
}
