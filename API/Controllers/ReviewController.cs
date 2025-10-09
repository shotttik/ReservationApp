using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Review;
using Application.Common.Results;
using Application.Interfaces;
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

    }
}