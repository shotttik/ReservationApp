using Domain.Entities.CompanyReleated;

namespace Domain.Interfaces.Repositories
{
    public interface ICompanyFAQCategoryRepository :IBaseRepository<CompanyFAQCategory>
    {
        Task<IEnumerable<CompanyFAQCategory>> GetAll(int companyID);
        Task<int> Count(int companyID);
    }
}
