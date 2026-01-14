using Domain.DTO;

namespace Application.Common.Responses
{
    public class CreateBookingByGuestResponse
    {
        public BookingDTO Booking { get; set; } = null!;
        public CreateGuestTokenResponse GuestToken { get; set; } = null!;
    }
}
