using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Booking
{
    public class GuestBookingAccessVerifyRequest
    {
        [Required]
        public required string Reference { get; set; }
        [Required]
        public required string Code { get; set; }
    }
}
