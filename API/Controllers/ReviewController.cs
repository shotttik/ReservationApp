using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Review;
using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO.Review;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/reviews")]
    [ApiController]
    [Tags("Reviews")]
    public class ReviewController :ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        /// <summary>
        /// Creates a review invite for a completed booking.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin, CompanyAdmin, CompanyEmployee</strong>
        /// - Booking must exist and be completed.<br/>
        /// - Invite must not already exist.<br/>
        /// Invite is valid for 14 days.
        /// </remarks>
        /// <param name="bookingId">Booking ID</param>
        [HttpPost("invites/bookings/{bookingId}")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ReviewInviteCreate)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateInvite(int bookingId)
        {
            var result = await reviewService.CreateInvite(bookingId);

            return result.ToResponse();
        }
        /// <summary>
        /// Submits a review using a valid invite.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>PublicUser</strong>
        /// - Invite must exist, belong to user, be valid, and not used before.
        /// </remarks>
        /// <param name="request">Review data</param>
        [HttpPost]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [HasPermission(Permission.ReviewCreate)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create(ReviewCreateRequest request)
        {
            var result = await reviewService.Create(request);

            return result.ToResponse();
        }
        /// <summary>
        /// Uploads one or more media files for the specified review.
        /// </summary>
        /// <remarks>
        /// This endpoint allows uploading multiple media files for review.
        ///
        /// <para><b>Required Roles:</b> PublicUser</para>
        /// <para><b>Max File Size:</b> 1 MB (1,048,576 bytes)</para>
        /// <para><b>Allowed File Types:</b> image/jpeg, image/png</para>
        /// </remarks>
        /// <param name="request">The request containing the media files to upload.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A result returns address of the image or failure of the upload operation.</returns>
        [HttpPost("medias")]
        [HasPermission(Permission.ReviewCreate)]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<List<int>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadMedia([FromForm] UploadReviewMediasRequest request, CancellationToken cancellationToken)
        {
            var result = await reviewService.UploadMedia(request, cancellationToken);

            return result.ToResponse();
        }

        /// <summary>
        /// Retrieves all currently open review invites for the authenticated user.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a list of review invites that the user is eligible to submit a review for. 
        /// The list is filtered by the user’s role and only includes invites that:
        /// - Have not yet been reviewed (`ClientReviewed = false`)
        /// - Are still within the review window (`CloseAt >= now`)
        ///
        /// <para><b>Role-based behavior:</b></para>
        /// <list type="bullet">
        ///     <item><b>PublicUser:</b> Returns invites where the authenticated user is the client.</item>
        ///     <item><b>CompanyEmployee / CompanyAdmin:</b> Returns invites where the authenticated user is the employee associated with the booking.</item>
        /// </list>
        /// <para><b>Required Roles:</b> PublicUser, CompanyEmployee, CompanyAdmin</para>
        /// </remarks>
        /// <returns>
        /// Returns a list of <see cref="ReviewInviteDTO"/> objects representing open review invites for the authenticated user.
        /// </returns>
        [HttpGet("open-invites")]
        [HasPermission(Permission.ReviewInviteReadLimited)]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<List<ReviewInviteDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOpenReviewInvites()
        {
            var result = await reviewService.GetOpenReviewInvites();

            return result.ToResponse();
        }
    }
}