using API.Attributes;
using Application.Common.Requests.Booking;
using Application.Common.Results;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/booking")]
    [ApiController]
    [Tags("Booking")]
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
        [HttpPost]
        [Authorize]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateRequest request)
        {
            var result = await bookingService.Create(request);

            return result.ToResponse();
        }

    }
}
