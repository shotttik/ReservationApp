using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Booking
{
    public class BookingVerificationRequest
    {
        [Required]
        public required string Code { get; set; }
    }
}
