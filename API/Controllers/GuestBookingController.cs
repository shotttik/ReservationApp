using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Booking;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Interfaces;
using Application.Services;
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
        private readonly IBookingService _bookingService;

        public GuestBookingController(
            IGuestBookingService guestBookingService, IBookingService bookingService)
        {
            _guestBookingService = guestBookingService;
            _bookingService = bookingService;
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
        /// <summary>
        /// Cancels an existing booking.
        /// </summary>
        /// <remarks>
        /// <b>Purpose:</b><br/>
        /// Cancels a booking by updating its status to <c>Canceled</c>.
        /// <br/><br/>
        /// <b>Behavior:</b>
        /// <ul>
        ///   <li>Booking status changes to <c>Canceled</c>.</li>
        ///   <li>Cancellation reason can be provided using <c>CancellationReason</c>.</li>
        ///   <li>If no cancellation reason is provided, the booking will still be cancelled.</li>
        /// </ul>
        /// <br/>
        /// <b>Important:</b>
        /// <ul>
        ///   <li>Accessible to the guest who created the booking or the authenticated user associated with the booking.</li>
        ///   <li>Only cancellable bookings can be cancelled.</li>
        /// </ul>
        /// <b>Cancellable if:</b>
        /// <ul>
        ///   <li>Booking status is <c>Pending</c>.</li>
        ///   <li>Booking status is <c>Accepted</c>.</li>
        /// </ul>
        /// <br/>
        /// Required role: Accessible by guest with valid access or authenticated booking owner.
        /// </remarks>
        /// <param name="id">The identifier of the booking to cancel.</param>
        /// <param name="request">Contains the optional cancellation reason.</param>
        /// <returns>
        /// Returns a <see cref="SuccessResponse"/> when the booking is successfully cancelled.
        /// Returns validation errors (400), forbidden (403), or not found (404).
        /// </returns>
        [HttpPatch("{id:int}/cancel")]
        [GuestOrUser]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel([FromRoute] int id, [FromBody] BookingCancelRequest? request)
        {
            var result = await _bookingService.CancelBooking(id, request);
            return result.ToResponse();
        }

        /// <summary>
        /// Reschedules an existing booking.
        /// </summary>
        /// <remarks>
        /// <b>Purpose:</b><br/>
        /// Reschedules a booking by updating its start time, employee, and/or service.
        /// <br/><br/>
        /// <b>Behavior:</b>
        /// <ul>
        ///   <li>Booking status changes to <c>Pending</c>.</li>
        ///   <li>Booking can be assigned to a new employee using <c>EmployeeId</c>.</li>
        ///   <li>Booking service can be changed using <c>ServiceId</c>.</li>
        /// </ul>
        /// <br/>
        /// <b>Important:</b>
        /// <ul>
        ///   <li>Accessible to the guest who created the booking or the authenticated user associated with the booking.</li>
        ///   <li>Only reschedulable bookings can be rescheduled.</li>
        /// </ul>
        /// <b>Reschedulable if:</b>
        /// <ul>
        ///   <li>Booking status allows rescheduling.</li>
        ///   <li>New start time is at least 30 minutes in the future.</li>
        /// </ul>
        /// <br/>
        /// Required role: Accessible by guest with valid access or authenticated booking owner.
        /// </remarks>
        /// <param name="id">The identifier of the booking to reschedule.</param>
        /// <param name="request">Contains the new service, employee, and start time.</param>
        /// <returns>
        /// Returns a <see cref="SuccessResponse"/> when the booking is successfully rescheduled.
        /// Returns validation errors (400), forbidden (403), or not found (404).
        /// </returns>
        [HttpPatch("{id:int}/reschedule")]
        [GuestOrUser]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Reschedule([FromRoute] int id, [FromBody] RescheduleBookingRequest request)
        {
            var result = await _bookingService.RescheduleBooking(id, request);
            return result.ToResponse();
        }
    }
}