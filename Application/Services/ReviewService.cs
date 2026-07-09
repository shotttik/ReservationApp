using Application.Common.Requests.Review;
using Application.Common.Results;
using Application.Extensions;
using Application.Extensions.Mappers;
using Application.Extensions.Mappers.Pagination;
using Application.Interfaces;
using Application.Options;
using Domain.Abstractions;
using Domain.DTO.Review;
using Domain.Entities.Common;
using Domain.Entities.ReviewReleated;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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
        private readonly MediaLimitsOptions _mediaLimitsOptions;

        public ReviewService(
            IReviewInviteRepository reviewInviteRepository,
            IBookingRepository bookingRepository,
            IAccessGuard accessGuard,
            IReviewRepository reviewRepository,
            IAuthService authService,
            IMediaRepository mediaRepository,
            IReviewMediaRepository reviewMediaRepository,
            IFileStorageService fileStorageService,
            IOptions<MediaLimitsOptions> mediaLimitsOptions)
        {
            this.reviewInviteRepository = reviewInviteRepository;
            this.bookingRepository = bookingRepository;
            this.accessGuard = accessGuard;
            this.reviewRepository = reviewRepository;
            this.authService = authService;
            this.mediaRepository = mediaRepository;
            this.reviewMediaRepository = reviewMediaRepository;
            this.fileStorageService = fileStorageService;
            _mediaLimitsOptions = mediaLimitsOptions.Value;
        }


        public async Task<Result> CreateInvite(int bookingId)
        {
            var booking = await bookingRepository.GetWithBranchAndReviewInvite(bookingId);
            if (booking == null)
            {
                return Result.Failure(BookingResults.NotFound);
            }
            var accessError = await accessGuard.EnsureAccessToBooking(booking.Id, booking.ClientID, booking.EmployeeID, booking.Branch.CompanyId);
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
                OpenAt = DateTime.UtcNow,
                CloseAt = DateTime.UtcNow.AddDays(14),
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

            if (reviewInvite.CloseAt < DateTime.UtcNow)
            {
                return Result.Failure(ReviewResults.InviteExpired);
            }

            if (reviewInvite.ClientReviewed)
            {
                return Result.Failure(ReviewResults.AlreadyReviewed);
            }

            var mediaExists = await mediaRepository.Exists(request.MediaIds);
            if (!mediaExists)
            {
                return Result.Failure(MediaResults.SomeMediaDontExists);
            }
            reviewInvite.ClientReviewed = true;
            reviewInvite.UpdateTimestamp();
            var review = request.MapToEntity();
            await reviewRepository.Add(review);

            var reviewMedia = request.MediaIds
                .Select(mediaId => new ReviewMedia
                {
                    MediaId = mediaId,
                    ReviewId = review.Id
                })
                .ToList();

            await reviewMediaRepository.AddRange(reviewMedia);

            return Result.Success(ReviewResults.ReviewCreated);
        }
        public async Task<Result<List<int>>> UploadMedia(UploadReviewMediaRequest request, CancellationToken cancellationToken)
        {
            var mediaIds = new List<int>();
            foreach (var item in request.Media)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var error = item.IsValidImage(_mediaLimitsOptions);
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
                    Domain.Enums.UploadSubFolder.ReviewMedia,
                    cancellationToken);

                var media = new Media()
                {
                    OriginalName = fileName,
                    RemoteUrl = WebpPath,
                    OriginalUrl = OriginalPath,
                    FileType = contentType,
                    FileSizeInBytes = item.Length
                };
                await mediaRepository.Add(media, cancellationToken);
                mediaIds.Add(media.Id);
            }

            return Result.Success(mediaIds);
        }
        public async Task<Result<IEnumerable<ReviewInviteDTO>>> GetReviewInvites()
        {
            var authUser = await authService.GetCurrentUser();

            var openInvites = await reviewInviteRepository.GetReviewInvites(authUser.UserAccountId, Enum.Parse<Role>(authUser.Role.Name));

            return openInvites.Select(e => e.MapToDTO()).ToList();
        }
        public async Task<Result<PagedList<ReviewDTO>>> RetrievePaged(PagedParameters parameters, CancellationToken cancellationToken)
        {
            bool isSuperAdmin = authService.IsInRole(Domain.Entities.User.Role.SuperAdmin.Name);

            var allowedFields = ReviewFieldMap.DtoToEntityPath(!isSuperAdmin);
            var errors = parameters.Validate(allowedFields, typeof(Review));
            if (errors.Any())
            {
                return Result.Failure<PagedList<ReviewDTO>>(PagedListResults.InvalidPagedParameters(errors.First()));
            }
            var reviews = await reviewRepository.RetrievePaged(
                parameters,
                !isSuperAdmin,
                cancellationToken);

            return reviews;
        }
        public async Task<Result<ReviewDTO>> ReviewUpdate(int id, ReviewUpdateRequest request)
        {
            var review = await reviewRepository.Get(id);
            if (review == null)
            {
                return Result.Failure<ReviewDTO>(ReviewResults.NotFound);
            }
            review.ApplyTo(request);

            await reviewRepository.Update(review);

            return review.MapToDTO();
        }
    }
}
