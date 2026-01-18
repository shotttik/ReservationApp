using Application.Extensions.Mappers;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Entities.Common;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Extensions;
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

        public async Task<List<Booking>> GetDataForAllActiveEmployees(int companyId, DateOnly startDate, DateOnly endDate)
        {
            return await _dbSet.Where(b =>
                b.CompanyID == companyId &&
                DateOnly.FromDateTime(b.StartTime) >= startDate &&
                DateOnly.FromDateTime(b.StartTime) <= endDate &&
                (
                b.Status == BookingStatus.Accepted ||
                b.Status == BookingStatus.Completed)
                ).ToListAsync();
        }

        public async Task<PagedList<BookingDTO>> RetrievePaged(PagedParameters parameters, CancellationToken cancellationToken)
        {
            var query = _dbSet.AsQueryable();

            query = query.ApplyQueryParamsAsync(parameters);

            var totalCount = await query.CountAsync(cancellationToken);

            var bookings = await query
                .Select(e => e.MapToDTO(false))
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<BookingDTO>(bookings, parameters.PageNumber, parameters.PageSize, totalCount);
        }

        public async Task<Booking?> GetWithReviewInvite(int bookingId)
        {
            var booking = await _dbSet
                .Where(e => e.ID == bookingId)
                .Include(e => e.ReviewInvite)
                .FirstOrDefaultAsync();

            return booking;
        }

        public async Task<BookingWithLatestPendingVerification?> GetWithGuestInfoAndLatestPendingVerification(int bookingId)
        {
            return await _dbSet
                .Where(b => b.ID == bookingId &&
                            b.GuestInfo != null &&
                            b.Status == BookingStatus.PendingVerification)
                .Include(b => b.GuestInfo)
                .Select(b => new BookingWithLatestPendingVerification(
                    b,
                    b.Verifications
                        .Where(v => v.VerifiedAt == null)
                        .OrderByDescending(v => v.CreatedAt)
                        .FirstOrDefault()
                ))
                .FirstOrDefaultAsync();
        }

        public async Task<BookingWithLatestPendingVerification?> GetContactUpdatableWithLatestPendingVerification(int bookingId)
        {
            return await _dbSet
                .Where(b => b.ID == bookingId &&
                            b.GuestInfo != null &&
                            (b.Status == BookingStatus.PendingVerification || b.Status == BookingStatus.Pending || b.Status == BookingStatus.Accepted))
                .Include(b => b.GuestInfo)
                .Select(b => new BookingWithLatestPendingVerification(
                    b,
                    b.Verifications
                        .Where(v => v.VerifiedAt == null)
                        .OrderByDescending(v => v.CreatedAt)
                        .FirstOrDefault()
                ))
                .FirstOrDefaultAsync();
        }

        public async Task<BookingWithLatestPendingVerification?> GetWithGuestInfoAndLatestPendingVerification(string reference, string contact)
        {
            return await _dbSet
                .Where(b => b.Reference == reference &&
                       b.GuestInfo != null &&
                       b.GuestInfo.Contact == contact &&
                       (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Accepted))
                .Include(b => b.GuestInfo)
                .Select(b => new BookingWithLatestPendingVerification(
                    b,
                    b.Verifications
                        .Where(v => v.VerifiedAt == null)
                        .OrderByDescending(v => v.CreatedAt)
                        .FirstOrDefault()
                ))
                .FirstOrDefaultAsync();
        }

        public async Task<BookingWithLatestPendingVerification?> GetWithGuestInfoAndLatestPendingVerification(string reference)
        {
            return await _dbSet
                .Where(b => b.Reference == reference && (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Accepted))
                .Select(b => new BookingWithLatestPendingVerification(
                    b,
                    b.Verifications
                        .Where(v => v.VerifiedAt == null)
                        .OrderByDescending(v => v.CreatedAt)
                        .FirstOrDefault()
                ))
                .FirstOrDefaultAsync();
        }
    }
}
