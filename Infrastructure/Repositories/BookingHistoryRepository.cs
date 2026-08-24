using Domain.Entities.Common;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookingHistoryRepository :BaseRepository<BookingHistory>, IBookingHistoryRepository
    {
        public BookingHistoryRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task AddWithoutSave(BookingHistory history)
        {
            await _dbSet.AddAsync(history);
        }

        public async Task<IEnumerable<BookingHistory>> GetAll(int bookingId, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking()
                    .Where(x => x.BookingId == bookingId)
                    .OrderByDescending(x => x.CreatedAt)
                    .ThenByDescending(x => x.Id)
                    .ToListAsync(cancellationToken);
        }
    }
}
