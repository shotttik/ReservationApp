using API.Attributes;
using Application.Authentication;
using Application.Common.Requests.Booking;
using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/bookings")]
    [ApiController]
    [Tags("Bookings")]
    public class BookingController :ControllerBase
    {
        private readonly IBookingService bookingService;

        public BookingController(IBookingService bookingService)
        {
            this.bookingService = bookingService;
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
            var result = await bookingService.CreateByClient(request);

            return result.ToResponse();
        }
        /// <summary>
        /// Create a new booking.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>SuperAdmin, CompanyAdmin.</strong><br/><br/>
        /// Creates a booking with the specified time, employee, client and service details.  
        /// Only available time slots for the employee and client will be accepted.  
        /// Returns a success message or validation failure.
        /// </remarks>
        /// <param name="request">Booking creation request with time ,service info and client info.</param>
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
            var result = await bookingService.CreateByAdmin(request);

            return result.ToResponse();
        }
        /// <summary>
        /// Public method. Returns confirmed bookings for all active employees,
        /// </summary>
        /// <remarks>
        /// Takes first day of week.
        /// Required role: <strong>Accessible by everyone</strong><br/><br/>
        /// </remarks>
        /// <param name="companyId">company id</param>
        /// <param name="targetDate">this is start date of week</param>
        /// <returns>List of BookingDTO when success or empty</returns>
        [HttpGet("companies/{companyId:int}")]
        [MapToApiVersion("1.0")]
        [Logging(LoggingType.General)]
        [ProducesResponseType(typeof(SuccessResponse<List<BookingDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetWeeklyPublicData(int companyId, [FromQuery] DateOnly targetDate)
        {
            var result = await bookingService.GetWeeklyPublicData(companyId, targetDate);

            return result.ToResponse();
        }
        /// <summary>
        /// Changes status for specific booking.
        /// </summary>
        /// <remarks>
        /// Completed booking status can't change.
        /// SuperAdmin - Can update any booking.
        /// CompanyAdmin - Can update only own company bookings.
        /// CompanyEmployee,PublicUser - Can update only own bookings.
        /// Required role: <strong>Accessible only authorized.</strong><br/><br/>
        /// </remarks>
        /// <param name="id">booking id</param>
        /// <param name="request">request of booking status change</param>
        /// <returns>Success message or error result</returns>
        [HttpPatch("{id:int}")]
        [MapToApiVersion("1.0")]
        [Logging(LoggingType.Full)]
        [HasPermission(Permission.BookingUpdate)]
        [ProducesResponseType(typeof(SuccessResponse<List<BookingDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] BookingStatusChangeRequest request)
        {
            var result = await bookingService.ChangeStatus(id, request);

            return result.ToResponse();
        }
    }
}
