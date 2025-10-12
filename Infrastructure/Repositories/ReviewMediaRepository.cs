using Domain.Entities.ReviewReleated;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReviewMediaRepository :IReviewMediaRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<ReviewMedia> _dbSet;
        public ReviewMediaRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
            this._dbSet = dbContext.Set<ReviewMedia>();
        }

        public async Task AddRange(IEnumerable<ReviewMedia> reviewMedias)
        {
            await _dbSet.AddRangeAsync(reviewMedias);
            await _dbContext.SaveChangesAsync();
        }
    }
}
