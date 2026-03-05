using Domain.Entities.Common;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SubscriptionPlanRepository :BaseRepository<SubscriptionPlan>, ISubscriptionPlanRepository
    {
        public SubscriptionPlanRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<SubscriptionPlan?> GetWithCompanySubscriptions(int id)
        {
            return await _dbSet.Where(s => s.Id == id)
                .Include(s => s.CompanySubscriptions)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> HasCompanySubscriptions(int planId)
        {
            return await dbContext.CompanySubscriptions
                .AnyAsync(x => x.SubscriptionPlanId == planId);
        }
    }
}
