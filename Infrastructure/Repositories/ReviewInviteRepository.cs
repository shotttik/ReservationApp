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
        public async Task<IEnumerable<ReviewInvite>> GetReviewInvites(int userAccountId, Role role)
        {
            return role switch
            {
                Role.SuperAdmin => await _dbSet
                    .ToArrayAsync(),

                Role.CompanyEmployee or Role.CompanyAdmin => await _dbSet
                    .Include(e => e.Booking)
                    .Where(e => e.Booking.EmployeeID == userAccountId)
                    .ToArrayAsync(),

                Role.PublicUser => await _dbSet
                    .Include(e => e.Booking)
                    .Where(e => e.Booking.ClientID == userAccountId)
                    .ToArrayAsync(),

                _ => []
            };
        }

        public async Task<ReviewInvite?> GetWithBooking(int id)
        {
            return await _dbSet
                .Where(e => e.Id == id)
                .Include(e => e.Booking)
                .FirstOrDefaultAsync();
        }
    }
}
