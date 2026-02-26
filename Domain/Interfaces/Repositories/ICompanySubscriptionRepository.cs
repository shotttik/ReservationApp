using Domain.Abstractions;
using Domain.DTO;
using Domain.Entities.CompanyReleated;

namespace Domain.Interfaces.Repositories
{
    public interface ICompanySubscriptionRepository :IBaseRepository<CompanySubscription>
    {
        Task<SubscriptionUsageDTO?> GetSubscriptionData(int companyId);
        Task<CompanySubscription?> GetByCompanyId(int companyId);
        Task<PagedList<CompanySubscriptionDTO>> RetrievePaged(PagedParameters parameters);
    }
}
