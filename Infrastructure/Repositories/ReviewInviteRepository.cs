using Domain.Entities.ReviewReleated;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReviewInviteRepository :BaseRepository<ReviewInvite>, IReviewInviteRepository
    {
        public ReviewInviteRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<ReviewInvite>> GetOpenReviewInvites(int userAccountId, Role role)
        {

            if (role == Role.CompanyEmployee || role == Role.CompanyAdmin)
                return await _dbSet
                    .Include(e => e.Booking)
                    .Where(e => e.Booking.EmployeeID == userAccountId
                     && e.ClientReviewed == false
                     && e.CloseAt >= DateTime.Now)
                    .ToArrayAsync();
            else if (role == Role.PublicUser)
            {
                return await _dbSet
                    .Include(e => e.Booking)
                    .Where(e => e.Booking.ClientID == userAccountId
                    && e.ClientReviewed == false
                    && e.CloseAt >= DateTime.Now)
                    .ToArrayAsync();
            }
            else
            {
                return [];
            }
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
