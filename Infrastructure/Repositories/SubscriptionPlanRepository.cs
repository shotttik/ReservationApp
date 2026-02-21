using Domain.Entities.Common;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class SubscriptionPlanRepository :BaseRepository<SubscriptionPlan>, ISubscriptionPlanRepository
    {
        public SubscriptionPlanRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
