using Application.Common.Requests.Review;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.Entities.ReviewReleated;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class ReviewService :IReviewService
    {
        private readonly IReviewInviteRepository reviewInviteRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IAccessGuard accessGuard;
        private readonly IReviewRepository reviewRepository;
        private readonly IAuthService authService;

        public ReviewService(
            IReviewInviteRepository reviewInviteRepository,
            IBookingRepository bookingRepository,
            IAccessGuard accessGuard,
            IReviewRepository reviewRepository,
            IAuthService authService)
        {
            this.reviewInviteRepository = reviewInviteRepository;
            this.bookingRepository = bookingRepository;
            this.accessGuard = accessGuard;
            this.reviewRepository = reviewRepository;
            this.authService = authService;
        }


        public async Task<Result> CreateInvite(int bookingId)
        {
            var booking = await bookingRepository.GetWithReviewInvite(bookingId);
            if (booking == null)
            {
                return Result.Failure(BookingResults.NotFound);
            }
            var accessError = await accessGuard.EnsureAccessToBooking(booking.ClientID, booking.EmployeeID, booking.CompanyID);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            if (booking.ClientID == null)
            {
                return Result.Failure(ReviewResults.BookingCreatedForUnauthenticatedClient);
            }
            if (booking.ReviewInvite != null)
            {
                return Result.Failure(ReviewResults.InviteAlreadySent);
            }

            var reviewInvite = new ReviewInvite()
            {
                BookingId = bookingId,
                OpenAt = DateTime.Now,
                CloseAt = DateTime.Now.AddDays(14),
            };

            await reviewInviteRepository.Add(reviewInvite);

            return Result.Success(ReviewResults.InvitedCreated);
        }

        public async Task<Result> Create(ReviewCreateRequest request)
        {
            var reviewInvite = await reviewInviteRepository.GetWithBooking(request.InviteId);
            if (reviewInvite == null)
            {
                return Result.Failure(ReviewResults.InviteDoesntExists);
            }

            var autUserAccountId = authService.GetUserAccountID();
            if (autUserAccountId != reviewInvite.Booking.ClientID)
            {
                return Result.Failure(ReviewResults.NotYourInvite);
            }

            if (reviewInvite.CloseAt < DateTime.Now)
            {
                return Result.Failure(ReviewResults.InviteExpired);
            }

            if (reviewInvite.ClientReviewed)
            {
                return Result.Failure(ReviewResults.AlreadyReviewed);
            }

            reviewInvite.ClientReviewed = true;
            reviewInvite.UpdateTimestamp();
            var review = request.MapToEntity();

            await reviewRepository.Add(review);

            return Result.Success(ReviewResults.ReviewCreated);
        }
    }
}
