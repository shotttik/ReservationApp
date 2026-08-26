using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Booking;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/bookings")]
    [ApiController]
    [Tags("Bookings")]
    public class BookingController :ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>
        /// Create a new booking by guest only.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>Accessible by everyone</strong><br/><br/>
        /// Creates a booking with the specified time, employee, client, service details, guest and verification info.  
        /// Only available time slots for the employee will be accepted.  
        /// Returns a success message or validation failure.
        /// </remarks>
        /// <param name="request">Booking creation request with guest info and verification info.</param>
        /// <returns>BookingDTO when success or error result.</returns>
        [Tags("Guest Bookings")]
        [HttpPost("guest")]
        [MapToApiVersion("1.0")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<CreateBookingByGuestResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateByGuest([FromBody] GuestBookingCreateRequest request)
        {
            var result = await _bookingService.CreateByGuest(request);

            return result.ToResponse();
        }
        /// <summary>
        /// Create a new booking.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>Accessible by everyone</strong><br/><br/>
        /// Creates a booking with the specified time, employee, and service details.  
        /// Only available time slots for the employee and client will be accepted.  
        /// Returns a success message or validation failure.
        /// </remarks>
        /// <param name="request">Booking creation request with time and service info.</param>
        /// <returns>BookingDTO when success or error result.</returns>
        [HttpPost("client")]
        [MapToApiVersion("1.0")]
        [Authorize]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<BookingDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateByClient([FromBody] ClientBookingCreateRequest request)
        {
            var result = await _bookingService.CreateByClient(request);

            return result.ToResponse();
        }
        /// <summary>
        /// Create a new booking.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin, CompanyAdmin.</strong><br/><br/>
        /// Required permission: <strong>BookingCreate</strong><br/><br/>
        /// Creates a booking with the specified time, employee, client and service details.  
        /// Only available time slots for the employee and client will be accepted.  
        /// Returns a success message or validation failure.
        /// </remarks>
        /// <param name="request">Booking creation request with guest info and verification info.</param>
        /// <returns>BookingDTO when success or error result.</returns>
        [HttpPost("admin")]
        [MapToApiVersion("1.0")]
        [HasPermission(Permission.BookingCreate)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<BookingDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateByAdmin([FromBody] AdminBookingCreateRequest request)
        {
            var result = await _bookingService.CreateByAdmin(request);

            return result.ToResponse();
        }
        /// <summary>
        /// Public method. Returns confirmed bookings for all active employees,
        /// </summary>
        /// <remarks>
        /// Takes first day of week.
        /// Required role: <strong>Accessible by everyone</strong><br/><br/>
        /// </remarks>
        /// <param name="branchId">company id</param>
        /// <param name="targetDate">this is start date of week</param>
        /// <returns>List of BookingDTO when success or empty</returns>
        [HttpGet("branches/{branchId:int}")]
        [MapToApiVersion("1.0")]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse<List<BookingDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetWeeklyPublicData(int branchId, [FromQuery] DateOnly targetDate)
        {
            var result = await _bookingService.GetWeeklyPublicData(branchId, targetDate);

            return result.ToResponse();
        }
        /// <summary>
        /// Changes status for specific booking.
        /// </summary>
        /// <remarks>
        /// SuperAdmin - Can update any booking.
        /// CompanyAdmin - Can update only own company bookings.
        /// CompanyEmployee,PublicUser - Can update only own bookings.
        /// Required role: <strong>Accessible only authorized.</strong><br/><br/>
        /// Required permission: <strong>BookingUpdate</strong><br/><br/>
        /// </remarks>
        /// <param name="id">booking id</param>
        /// <param name="request">request of booking status change</param>
        /// <returns>Success message or error result</returns>
        [HttpPatch("{id:int}/status")]
        [MapToApiVersion("1.0")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.BookingUpdate)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] BookingStatusChangeRequest request)
        {
            var result = await _bookingService.ChangeStatus(id, request);

            return result.ToResponse();
        }

        /// <summary>
        /// Retrieves a paginated list of bookings.
        /// </summary>
        /// <remarks>
        /// SuperAdmin - Can retrieve all bookings.<br/>
        /// CompanyAdmin - Can retrieve bookings for their own company.<br/>
        /// CompanyEmployee - Can retrieve their own bookings.<br/>
        /// PublicUser - Can retrieve their own bookings.<br/>
        /// Required role: <strong>Only Authorized Users</strong><br/><br/>
        /// <b>Paging and filtering parameters:</b><br/>
        /// <b>Sortable / Filterable Fields:</b>
        /// <ul>
        /// <li><c>ID</c></li>
        /// <li><c>ClientID</c></li>
        /// <li><c>EmployeeID</c></li>
        /// <li><c>CompanyID</c></li>
        /// <li><c>ServiceName</c></li>
        /// <li><c>StartTime</c></li>
        /// <li><c>EndTimeExpected</c></li>
        /// <li><c>EndTime</c></li>
        /// <li><c>PriceExpected</c></li>
        /// <li><c>Discount</c></li>
        /// <li><c>PriceFinal</c></li>
        /// <li><c>Status</c></li>
        /// <li><c>CreatedAt</c></li>
        /// <li><c>UpdatedAt</c></li>
        /// </ul>
        /// Filtering Example: name~=Company 4||email~=Company40,id==3107
        /// </remarks>
        /// <param name="parameters">Pagination parameters including page number, size, and search filters.</param>
        /// <param name="cancellationToken">Request cancellation token.</param>
        /// <returns>Paged list of company records.</returns>
        [HttpGet()]
        [Authorize]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<PagedList<BookingDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RetrievePaged([FromQuery] PagedParameters parameters, CancellationToken cancellationToken)
        {
            var result = await _bookingService.RetrievePaged(parameters, cancellationToken);

            return result.ToResponse();
        }
        /// <summary>
        /// Retrieves a booking by its identifier.
        /// </summary>
        /// <remarks>
        /// Returns the full BookingFullDTO for the specified booking id. The caller must be a guest or an authenticated user.
        /// Example: GET /api/v1/bookings/123
        /// </remarks>
        /// <param name="id">The unique identifier of the booking to retrieve.</param>
        /// <returns>
        /// 200: Success — Returns a SuccessResponse containing the BookingDTO.
        /// 400: Bad Request — The provided id is invalid.
        /// 404: Not Found — No booking exists with the specified id.
        /// </returns>
        /// <response code="200">Successful operation. Returns booking details.</response>
        /// <response code="400">Bad request - invalid id supplied.</response>
        /// <response code="404">Booking not found.</response>
        [HttpGet("{id:int}")]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<BookingFullDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [GuestOrUser]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _bookingService.GetFullData(id);
            return result.ToResponse();
        }

        /// <summary>
        /// Deletes the booking
        /// </summary>
        /// <remarks>
        /// This endpoint allows only the SuperAdmin to permanently delete the bookingn).
        /// Required role: <strong>SuperAdmin</strong><br/><br/>
        /// Required permission: <strong>BookingDelete</strong>
        /// </remarks>
        /// <param name="id">The ID of the booking in the route.</param>
        /// <returns>No content on success; appropriate error response on failure.</returns>
        [HttpDelete("{id:int}")]
        [HasPermission(Permission.BookingDelete)]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _bookingService.Delete(id);
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
        /// Required role: Accessible by guest with valid access or authenticated booking owner (could be guest).
        /// </remarks>
        /// <param name="id">The identifier of the booking to reschedule.</param>
        /// <param name="request">Contains the new service, employee, and start time.</param>
        /// <returns>
        /// Returns a <see cref="SuccessResponse"/> when the booking is successfully rescheduled.
        /// Returns validation errors (400), forbidden (403), or not found (404).
        /// </returns>
        [HttpPatch("{id:int}/reschedule")]
        [GuestOrUser]
        [EnableRateLimiting("fixed")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Reschedule([FromRoute] int id, [FromBody] RescheduleBookingRequest request)
        {
            var result = await _bookingService.RescheduleBooking(id, request);
            return result.ToResponse();
        }
        /// <summary>
        /// Updates the note for a specific booking.
        /// </summary>
        /// <remarks> 
        /// Required role: Accessible by guest with valid access or authenticated booking owner (could be guest).
        /// </remarks>
        /// <param name="id">The ID of the booking to update.</param>
        /// <param name="request">The request containing the updated note.</param>
        /// <returns>Success message or error result.</returns>
        /// <response code="200">Successfully updated the booking note.</response>
        [HttpPatch("{id:int}/note")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<BookingDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [GuestOrUser]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateNote([FromRoute] int id, [FromBody] UpdateBookingNoteRequest request)

        {
            var result = await _bookingService.UpdateNote(id, request);
            return result.ToResponse();
        }


        /// <summary>
        /// Retrieves the history for a specific booking.
        /// </summary>
        /// <param name="id">The identifier of the booking to retrieve history for.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> which on success contains a success response with a list of <see cref="BookingHistoryDto"/> items,
        /// or a <see cref="ProblemDetails"/> on error.
        /// </returns>
        /// <response code="200">Returns the booking history.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpGet("{id:int}/history")]
        [GuestOrUser]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<List<BookingHistoryDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetHistory(
            int id,
            CancellationToken cancellationToken)
        {

            var result = await _bookingService
                .GetBookingHistoryAsync(
                    id,
                    cancellationToken);

            return result.ToResponse();
        }
    }
}
