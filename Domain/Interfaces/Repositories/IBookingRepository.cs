using Domain.Abstractions;
using Domain.DTO;
using Domain.Entities.Common;

namespace Domain.Interfaces.Repositories
{
    public interface IBookingRepository :IBaseRepository<Booking>
    {
        Task<bool> HasBookingOverlap(int userId, DateTime start, DateTime end, bool asEmployee);
        Task<List<Booking>> GetDataForAllActiveEmployees(int companyId, DateOnly startDate, DateOnly endDate);
        Task<PagedList<BookingDTO>> RetrievePaged(
            PagedParameters parameters,
            CancellationToken cancellationToken);
        Task<Booking?> GetWithReviewInvite(int bookingId);
    }
}
