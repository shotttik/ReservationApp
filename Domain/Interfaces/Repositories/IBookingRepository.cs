using Domain.Abstractions;
using Domain.DTO;
using Domain.Entities.Common;

namespace Domain.Interfaces.Repositories
{
    public interface IBookingRepository :IBaseRepository<Booking>
    {
        Task<bool> HasBookingOverlap(int userId, DateTime start, DateTime end, int? bookingId, bool asEmployee);
        Task<List<Booking>> GetDataForAllActiveEmployees(int branchId, DateOnly startDate, DateOnly endDate);
        Task<PagedList<BookingDTO>> RetrievePaged(
            PagedParameters parameters,
            CancellationToken cancellationToken);
        Task<Booking?> GetWithReviewInvite(int bookingId);
        Task<Booking?> GetWithBranchAndReviewInvite(int bookingId);
        Task<Booking?> GetWithBranch(int bookingId);
        Task<BookingWithLatestPendingVerification?> GetWithGuestInfoAndLatestPendingVerification(int bookingId);
        Task<BookingWithLatestPendingVerification?> GetContactUpdatableWithLatestPendingVerification(int bookingId);
        Task<BookingWithLatestPendingVerification?> GetWithGuestInfoAndLatestPendingVerification(string reference, string contact);
        Task<BookingWithLatestPendingVerification?> GetWithGuestInfoAndLatestPendingVerification(string reference);
    }
}
