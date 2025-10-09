using Domain.Entities.ReviewReleated;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReviewInviteRepository :BaseRepository<ReviewInvite>, IReviewInviteRepository
    {
        public ReviewInviteRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<ReviewInvite?> GetWithBooking(int id)
        {
            return await _dbSet
                .Where(e => e.ID == id)
                .Include(e => e.Booking)
                .FirstOrDefaultAsync();
        }
    }
}
