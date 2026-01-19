using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Booking;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/guest-bookings")]
    [ApiController]
    [Tags("Guest Bookings")]
    public class GuestBookingController :ControllerBase
    {
        private readonly IGuestBookingService _guestBookingService;

        public GuestBookingController(
            IGuestBookingService guestBookingService)
        {
            _guestBookingService = guestBookingService;
        }
        /// <summary>
        /// Verifies a guest booking using a one-time verification code.
        /// </summary>
        /// <remarks>
        /// <b>Purpose:</b><br/>
        /// Confirms that the guest owns the provided contact (email or phone)  
        /// and finalizes the booking verification process.
        /// <br/><br/>
        /// <b>When to use:</b>
        /// <ul>
        /// <li>Immediately after a guest creates a booking</li>
        /// <li>After updating guest contact information</li>
        /// </ul>
        /// <br/>
        /// <b>Workflow:</b>
        /// <ol>
        /// <li>Guest creates booking → status becomes <c>PendingVerification</c></li>
        /// <li>System sends verification code via SMS or Email</li>
        /// <li>This endpoint verifies the code</li>
        /// <li>Booking status changes to <c>Pending</c></li>
        /// </ol>
        /// <br/>
        /// <b>Rules:</b>
        /// <ul>
        /// <li>Verification code must not be expired</li>
        /// <li>Only the latest pending verification is accepted</li>
        /// <li>Invalid or expired codes are rejected</li>
        /// </ul>
        /// <br/>
        /// Required role: Accessible by everyone
        /// </remarks>
        /// <param name="id">Booking ID to verify.</param>
        /// <param name="request">Verification code provided by the guest.</param>
        /// <returns>Success message or validation error.</returns>
        [HttpPost("{id:int}/verify")]
        [GuestOrUser]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Verify([FromRoute] int id, [FromBody] BookingVerificationRequest request)
        {
            var result = await _guestBookingService.Verify(id, request);
            return result.ToResponse();
        }

        /// <summary>
        /// Resends a verification code for a guest booking.
        /// </summary>
        /// <remarks>
        /// <b>Purpose:</b><br/>
        /// Sends a new verification code when the previous one has expired.
        /// <br/><br/>
        /// <b>When to use:</b>
        /// <ul>
        /// <li>Guest did not receive the original code</li>
        /// <li>The previous verification code expired</li>
        /// </ul>
        /// <br/>
        /// <b>Important:</b>
        /// <ul>
        /// <li>A new code is sent only if the previous one is expired</li>
        /// <li>Spam prevention is enforced</li>
        /// </ul>
        /// <br/>
        /// Required role: Accessible by everyone
        /// </remarks>
        /// <param name="id">Booking ID.</param>
        /// <returns>Success message or validation error.</returns>
        [HttpPost("{id:int}/verify/resend-code")]
        [GuestOrUser]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ResendVerificationCode([FromRoute] int id)
        {
            var result = await _guestBookingService.ResendVerificationCode(id);
            return result.ToResponse();
        }
        /// <summary>
        /// Sends a guest access verification code using booking reference.
        /// </summary>
        /// <remarks>
        /// <b>Purpose:</b><br/>
        /// Allows a guest to regain access to a booking when their guest token
        /// is missing or expired.
        /// <br/><br/>
        /// <b>When to use:</b>
        /// <ul>
        /// <li>Guest lost their guest JWT token</li>
        /// <li>Guest token expired</li>
        /// </ul>
        /// <br/>
        /// <b>Workflow:</b>
        /// <ol>
        /// <li>Guest provides booking reference + contact</li>
        /// <li>System sends verification code</li>
        /// <li>Guest verifies code to receive new guest token</li>
        /// </ol>
        /// <br/>
        /// Required role: Accessible by everyone
        /// </remarks>
        /// <param name="request">Booking reference and guest contact.</param>
        /// <returns>Success message or validation error.</returns>
        [HttpPost("access/send-code")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendGuestAccessCode([FromBody] GuestBookingAccessRequest request)
        {
            var result = await _guestBookingService.SendGuestAccessCode(request);
            return result.ToResponse();
        }
        /// <summary>
        /// Verifies guest access code and issues a guest JWT token.
        /// </summary>
        /// <remarks>
        /// <b>Purpose:</b><br/>
        /// Validates the guest access verification code and generates
        /// a temporary guest JWT token.
        /// <br/><br/>
        /// <b>When to use:</b>
        /// <ul>
        /// <li>After calling <c>SendGuestAccessCode</c></li>
        /// </ul>
        /// <br/>
        /// <b>Result:</b>
        /// <ul>
        /// <li>Returns guest JWT token</li>
        /// <li>Token is scoped only to the specific booking</li>
        /// </ul>
        /// <br/>
        /// Required role: Accessible by everyone
        /// </remarks>
        /// <param name="request">Booking reference and verification code.</param>
        /// <returns>Guest token and expiration info.</returns>
        [HttpPost("access/verify")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<CreateGuestTokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> VerifyGuestAccess([FromBody] GuestBookingAccessVerifyRequest request)
        {
            var result = await _guestBookingService.VerifyGuestAccess(request);
            return result.ToResponse();
        }
        /// <summary>
        /// Updates guest contact information and triggers re-verification.
        /// </summary>
        /// <remarks>
        /// <b>Purpose:</b><br/>
        /// Allows a guest to change their email or phone number.
        /// <br/><br/>
        /// <b>Behavior:</b>
        /// <ul>
        /// <li>Booking status changes to <c>PendingVerification</c></li>
        /// <li>A new verification code is sent to the new contact</li>
        /// <li>Old contact remains active until verification succeeds</li>
        /// </ul>
        /// <br/>
        /// <b>Important:</b>
        /// <ul>
        /// <li>Only one active verification is allowed at a time</li>
        /// <li>Spam prevention applies</li>
        /// </ul>
        /// <br/>
        /// Required role: Accessible by guest with valid access
        /// </remarks>
        /// <param name="id">Booking ID.</param>
        /// <param name="request">New contact information.</param>
        /// <returns>Success message or validation error.</returns>
        [HttpPost("{id:int}/guest-contact")]
        [Logging(LoggingType.Full)]
        [GuestOrUser]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateGuestInfoContact([FromRoute] int id, [FromBody] BookingGuestInfoContactUpdateRequest request)
        {
            var result = await _guestBookingService.UpdateGuestInfoContact(id, request);
            return result.ToResponse();
        }
    }
}