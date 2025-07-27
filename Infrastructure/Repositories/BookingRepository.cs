using Domain.Entities.Common;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookingRepository :BaseRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<bool> HasBookingOverlap(int userId, DateTime start, DateTime end, bool asEmployee)
        {
            var now = DateTime.UtcNow.Date;
            return await _dbSet
                .Where(b =>
                    (asEmployee ? b.EmployeeID == userId : b.ClientID == userId) &&
                    (b.Status == BookingStatus.Accepted) &&
                    b.StartTime < end &&
                    b.EndTimeExpected > start &&
                    b.StartTime.Date >= now
                    )
                .AnyAsync();
        }
    }
}
