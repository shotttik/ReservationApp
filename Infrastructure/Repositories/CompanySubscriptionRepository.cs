using Domain.Entities.CompanyReleated;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class CompanySubscriptionRepository :BaseRepository<CompanySubscription>, ICompanySubscriptionRepository
    {
        public CompanySubscriptionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
