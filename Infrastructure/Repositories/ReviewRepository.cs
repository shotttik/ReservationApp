using Domain.Entities.ReviewReleated;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class ReviewRepository :BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext) { }

    }
}
