using Domain.Entities.Common;

namespace Domain.Interfaces.Repositories
{
    public interface ISubscriptionPlanRepository :IBaseRepository<SubscriptionPlan>
    {
        Task<SubscriptionPlan?> GetWithCompanySubscriptions(int id);
        Task<bool> HasCompanySubscriptions(int planId);
    }
}
