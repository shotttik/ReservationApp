using Application.Extensions.Mappers;
using Domain.Abstractions;
using Domain.DTO;
using Domain.DTO.User;
using Domain.Entities.Common;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Repositories
{
    public class BookingRepository :BaseRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task AddWithoutSave(Booking entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task<bool> HasBookingOverlap(int userId, DateTime start, DateTime end, int? bookingId, bool asEmployee)
        {
            var now = DateTime.UtcNow;
            var query = _dbSet
                 .Where(b =>
                     (b.Status == BookingStatus.Accepted) &&
                     b.StartTime < end && // overlap check
                     b.EndTimeExpected > start &&
                     b.StartTime >= now // only future bookings
                     ).AsQueryable();

            if (asEmployee)
                query = query.Where(b => b.EmployeeID == userId);
            else
                query = query.Where(b => b.ClientID == userId);

            // exclude the current booking from the overlap check.
            // this is required when rescheduling/updating a booking,
            // otherwise it would always be detected as overlapping with itself.
            if (bookingId is not null)
            {
                query = query.Where(b => b.Id != bookingId);
            }
            return await query.AnyAsync();
        }

        public async Task<List<Booking>> GetDataForAllActiveEmployees(int branchId, DateOnly startDate, DateOnly endDate)
        {
            return await _dbSet.Where(b =>
                b.BranchId == branchId &&
                DateOnly.FromDateTime(b.StartTime) >= startDate &&
                DateOnly.FromDateTime(b.StartTime) <= endDate &&
                (
                b.Status == BookingStatus.Accepted ||
                b.Status == BookingStatus.Completed)
                ).Include(e => e.Service).ToListAsync();
        }

        public async Task<PagedList<BookingDTO>> RetrievePaged(PagedParameters parameters, AuthUser authUser, CancellationToken cancellationToken)
        {
            var query = _dbSet.
                Include(e => e.Service).
                Include(e => e.Branch).
                AsQueryable();

            query = ApplyAuthUserFilter(query, authUser);
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
                .Where(e => e.Id == bookingId)
                .Include(e => e.ReviewInvite)
                .FirstOrDefaultAsync();

            return booking;
        }
        public async Task<Booking?> GetWithBranchAndReviewInvite(int bookingId)
        {
            var booking = await _dbSet
                .Where(e => e.Id == bookingId)
                .Include(e => e.ReviewInvite)
                .Include(e => e.Branch)
                .FirstOrDefaultAsync();

            return booking;
        }
        public async Task<Booking?> GetWithBranch(int bookingId)
        {
            var booking = await _dbSet
                .Where(e => e.Id == bookingId)
                .Include(e => e.Branch)
                .Include(e => e.Service)
                .FirstOrDefaultAsync();

            return booking;
        }
        public async Task<Booking?> GetFullData(int bookingId)
        {
            var booking = await _dbSet
                .Where(e => e.Id == bookingId)
                .Include(e => e.Employee)
                    .ThenInclude(e => e.Role)
                .Include(e => e.Client)
                    .ThenInclude(c => c!.Role)
                .Include(e => e.Branch)
                    .ThenInclude(b => b.Company)
                .Include(e => e.Service)
                .Include(e => e.GuestInfo)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return booking;
        }
        public async Task<BookingWithLatestPendingVerification?> GetWithGuestInfoAndLatestPendingVerification(int bookingId)
        {
            return await _dbSet
                .Where(b => b.Id == bookingId &&
                            b.GuestInfo != null &&
                            b.Status == BookingStatus.PendingVerification)
                .Include(b => b.GuestInfo)
                .Include(b => b.Branch)
                .Select(b => new BookingWithLatestPendingVerification(
                    b,
                    b.Verifications
                        .Where(v => v.VerifiedAt == null)
                        .OrderByDescending(v => v.CreatedAt)
                        .FirstOrDefault()
                ))
                .AsSplitQuery()
                .FirstOrDefaultAsync();
        }

        public async Task<BookingWithLatestPendingVerification?> GetContactUpdatableWithLatestPendingVerification(int bookingId)
        {
            return await _dbSet
                .Where(b => b.Id == bookingId &&
                            b.GuestInfo != null &&
                            (b.Status == BookingStatus.PendingVerification || b.Status == BookingStatus.Pending || b.Status == BookingStatus.Accepted))
                .Include(b => b.GuestInfo)
                .Include(b => b.Branch)
                .Select(b => new BookingWithLatestPendingVerification(
                    b,
                    b.Verifications
                        .Where(v => v.VerifiedAt == null)
                        .OrderByDescending(v => v.CreatedAt)
                        .FirstOrDefault()
                ))
                .AsSplitQuery()
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
                .Include(b => b.Branch)
                .Select(b => new BookingWithLatestPendingVerification(
                    b,
                    b.Verifications
                        .Where(v => v.VerifiedAt == null)
                        .OrderByDescending(v => v.CreatedAt)
                        .FirstOrDefault()
                ))
                .AsSplitQuery()
                .FirstOrDefaultAsync();
        }

        public async Task<BookingWithLatestPendingVerification?> GetWithGuestInfoAndLatestPendingVerification(string reference)
        {
            return await _dbSet
                .Where(b => b.Reference == reference && (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Accepted))
                .Include(b => b.Branch)
                .Select(b => new BookingWithLatestPendingVerification(
                    b,
                    b.Verifications
                        .Where(v => v.VerifiedAt == null)
                        .OrderByDescending(v => v.CreatedAt)
                        .FirstOrDefault()
                ))
                .FirstOrDefaultAsync();
        }
        public async Task<bool> HasFutureBooking(int employeeId, int [] serviceIds)
        {
            return await _dbSet.Where(b => b.EmployeeID == employeeId
                && serviceIds.Contains(b.ServiceID)
                && b.StartTime > DateTime.UtcNow).AnyAsync();
        }
        private IQueryable<Booking> ApplyAuthUserFilter(
        IQueryable<Booking> query,
        AuthUser authUser)
        {
            switch (authUser.Role.Id)
            {
                case (int)Role.SuperAdmin:

                    break;
                case (int)Role.PublicUser:
                    query = query.Where(b => b.ClientID == authUser.UserAccountId);
                    break;
                case (int)Role.CompanyAdmin:
                    query = query.Where(b => b.Branch.CompanyId == authUser.CompanyId);
                    break;
                case (int)Role.CompanyEmployee:
                    query = query.Where(b => b.Branch.CompanyId == authUser.CompanyId && b.EmployeeID == authUser.UserAccountId);
                    break;
            }
            return query;
        }
    }
}
