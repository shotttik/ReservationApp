using Domain.DTO;

namespace Application.Common.Responses
{
    public class CreateBookingByGuestResponse
    {
        public BookingDTO Booking { get; set; } = null!;
        public string GuestToken { get; set; } = null!;
    }
}
