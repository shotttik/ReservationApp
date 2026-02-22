using Domain.DTO;
using Domain.Entities.CompanyReleated;

namespace Domain.Interfaces.Repositories
{
    public interface ICompanySubscriptionRepository :IBaseRepository<CompanySubscription>
    {
        Task<SubscriptionUsageDTO?> GetSubscriptionData(int companyId);
    }
}
