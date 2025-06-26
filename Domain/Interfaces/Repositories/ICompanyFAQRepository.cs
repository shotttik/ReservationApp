using Domain.Entities.CompanyReleated;

namespace Domain.Interfaces.Repositories
{
    public interface ICompanyFAQRepository :IBaseRepository<CompanyFAQ>
    {
        Task<IEnumerable<CompanyFAQ>> GetAll(int companyID, int? categoryID);
        Task<int> Count(int categoryID);
        Task<CompanyFAQ?> GetFull(int id);
    }
}
