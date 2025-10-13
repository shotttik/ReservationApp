using Application.Common.Requests.Review;
using Application.Common.Results;
using Application.Extensions;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO.Review;
using Domain.Entities.Common;
using Domain.Entities.ReviewReleated;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class ReviewService :IReviewService
    {
        private readonly IReviewInviteRepository reviewInviteRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IAccessGuard accessGuard;
        private readonly IReviewRepository reviewRepository;
        private readonly IAuthService authService;
        private readonly IMediaRepository mediaRepository;
        private readonly IReviewMediaRepository reviewMediaRepository;
        private readonly IFileStorageService fileStorageService;
        private readonly IConfiguration configuration;

        public ReviewService(
            IReviewInviteRepository reviewInviteRepository,
            IBookingRepository bookingRepository,
            IAccessGuard accessGuard,
            IReviewRepository reviewRepository,
            IAuthService authService,
            IMediaRepository mediaRepository,
            IReviewMediaRepository reviewMediaRepository,
            IFileStorageService fileStorageService,
            IConfiguration configuration)
        {
            this.reviewInviteRepository = reviewInviteRepository;
            this.bookingRepository = bookingRepository;
            this.accessGuard = accessGuard;
            this.reviewRepository = reviewRepository;
            this.authService = authService;
            this.mediaRepository = mediaRepository;
            this.reviewMediaRepository = reviewMediaRepository;
            this.fileStorageService = fileStorageService;
            this.configuration = configuration;
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

            var mediasExists = await mediaRepository.Exists(request.MediaIds);
            if (!mediasExists)
            {
                return Result.Failure(MediaResults.SomeMediaDontExists);
            }
            reviewInvite.ClientReviewed = true;
            reviewInvite.UpdateTimestamp();
            var review = request.MapToEntity();
            await reviewRepository.Add(review);

            var reviewMedias = request.MediaIds
                .Select(mediaId => new ReviewMedia
                {
                    MediaId = mediaId,
                    ReviewId = review.ID
                })
                .ToList();

            await reviewMediaRepository.AddRange(reviewMedias);

            return Result.Success(ReviewResults.ReviewCreated);
        }
        public async Task<Result<List<int>>> UploadMedia(UploadReviewMediasRequest request, CancellationToken cancellationToken)
        {
            var mediaIds = new List<int>();
            foreach (var item in request.Medias)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var error = item.IsValidImage(configuration);
                if (error != Error.None)
                {
                    return Result.Failure<List<int>>(error);
                }
                var fileName = item.FileName;
                var contentType = item.ContentType;
                var fileStream = item.OpenReadStream();

                (string OriginalPath, string WebpPath) = await fileStorageService.UploadWithWebp(
                    fileStream,
                    fileName,
                    contentType,
                    Domain.Enums.UploadSubFolder.CompanyImages,
                    cancellationToken);

                var media = new Media()
                {
                    OriginalName = fileName,
                    RemoteUrl = WebpPath,
                    FileType = contentType,
                    FileSizeInBytes = item.Length
                };
                await mediaRepository.Add(media, cancellationToken);
                mediaIds.Add(media.ID);
            }

            return Result.Success(mediaIds);
        }
        public async Task<Result<IEnumerable<ReviewInviteDTO>>> GetOpenReviewInvites()
        {
            var authUser = await authService.GetCurrentUser();
            var userAccountId = authService.GetUserAccountID();

            var openInvites = await reviewInviteRepository.GetOpenReviewInvites(userAccountId, Enum.Parse<Role>(authUser.Role.Name));

            return openInvites.Select(e => e.MapToDTO()).ToList();
        }
    }
}
