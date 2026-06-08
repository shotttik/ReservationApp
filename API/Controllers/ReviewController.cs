using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Review;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO.Review;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
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
        /// Required role: <strong>SuperAdmin, CompanyAdmin, CompanyEmployee</strong><br/><br/>
        /// Required permission: <strong>ReviewInviteCreate</strong>
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
        /// Required role: <strong>PublicUser</strong><br/><br/>
        /// Required permission: <strong>ReviewCreate</strong>
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
        /// <br/><br/>
        /// <para><b>Required Permission:</b> ReviewCreate</para>
        /// <para><b>Max File Size:</b> 1 MB (1,048,576 bytes)</para>
        /// <para><b>Allowed File Types:</b> image/jpeg, image/png</para>
        /// </remarks>
        /// <param name="request">The request containing the media files to upload.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A result returns address of the image or failure of the upload operation.</returns>
        [HttpPost("media")]
        [HasPermission(Permission.ReviewCreate)]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<List<int>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadMedia([FromForm] UploadReviewMediaRequest request, CancellationToken cancellationToken)
        {
            var result = await reviewService.UploadMedia(request, cancellationToken);

            return result.ToResponse();
        }

        /// <summary>
        /// Retrieves all currently review invites for the authenticated users.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a list of review invites that the user is eligible to submit(ed) a review for. 
        /// The list is filtered by the user’s role.:
        ///
        /// <para><b>Role-based behavior:</b></para>
        /// <list type="bullet">
        ///     <item><b>PublicUser:</b> Returns invites where the authenticated user is the client.</item>
        ///     <item><b>CompanyEmployee / CompanyAdmin:</b> Returns invites where the authenticated user is the employee associated with the booking.</item>
        ///     <item><b>SuperAdmin:</b> Returns all invites.</item>
        /// </list>
        /// <para><b>Required Roles:</b>Any Authenticated role</para>
        /// </remarks>
        /// <returns>
        /// Returns a list of <see cref="ReviewInviteDTO"/> objects representing open review invites for the authenticated user.
        /// </returns>
        [HttpGet("invites")]
        [Authorize]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<List<ReviewInviteDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOpenReviewInvites()
        {
            var result = await reviewService.GetReviewInvites();

            return result.ToResponse();
        }
        /// <summary>
        /// Retrieves a paginated list of reviews.
        /// </summary>
        /// <remarks>
        /// This endpoint allows retrieving reviews with paging, sorting, and filtering options.
        ///
        /// <para><b>Required role:</b> Accessible by everyone.</para>
        ///
        /// For SuperAdmin will be retrieved any reviews, for any other will be returned only published
        /// 
        /// <para><b>Paging and filtering parameters:</b></para>
        /// - Page number: `parameters.PageNumber`
        /// - Page size: `parameters.PageSize`
        /// - Filter string: `parameters.Filter` (e.g., "Status==Pending,ClientId=123")
        ///
        /// <para><b>Sortable / Filterable Fields:</b></para>
        /// <ul>
        /// <li><c>ID</c></li>
        /// <li><c>Overall</c></li>
        /// <li><c>Locale</c></li>
        /// <li><c>PublishedAt</c></li>
        /// <li><c>CreatedAt</c></li>
        /// <li><c>UpdatedAt</c></li>
        /// <li><c>ClientId</c></li>
        /// <li><c>EmployeeId</c></li>
        /// <li><c>CompanyId</c></li>
        /// <li><c>Status</c></li> This filter Works only For SuperAdmin
        /// </ul>
        /// </remarks>
        /// <param name="parameters">Pagination parameters including page number, page size, and search filters.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the request.</param>
        /// <returns>Returns containing a paged list of reviews. </returns>
        [HttpGet()]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<PagedList<ReviewDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrievePaged([FromQuery] PagedParameters parameters, CancellationToken cancellationToken)
        {
            var result = await reviewService.RetrievePaged(parameters, cancellationToken);

            return result.ToResponse();
        }

        /// <summary>
        ///     Updates user's submited review
        /// </summary>
        /// <remarks>
        /// All fields must be presented , its PUT bekhtangjy, sxva velebis update argvhirdeba
        /// 
        /// <para><b>Required role:</b>SuperAdmin.</para>
        /// <para><b>Required permission:</b> ReviewUpdate.</para>
        /// </remarks>
        /// <param name="id">id of the review</param>
        /// <param name="request">request body for params that will update(all must present)</param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.ReviewUpdate)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<PagedList<ReviewDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] ReviewUpdateRequest request)
        {
            var result = await reviewService.ReviewUpdate(id, request);
            return result.ToResponse();
        }
    }
}