using Domain.Entities.Common;

namespace Domain.Interfaces.Repositories
{
    public interface IBookingHistoryRepository :IBaseRepository<BookingHistory>
    {
        Task AddWithoutSave(BookingHistory history);
        Task<IEnumerable<BookingHistory>> GetAll(int bookingId, CancellationToken cancellationToken);
    }
}
