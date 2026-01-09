namespace Application.Common.Requests.Booking
{
    public class GuestBookingCreateRequest :ClientBookingCreateRequest
    {
        public required BookingGuestInfoCreateRequest GuestInfo { get; set; }
    }
}
