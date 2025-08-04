using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Booking
{
    public class AdminBookingCreateRequest :ClientBookingCreateRequest
    {
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public required string ClientEmail { get; set; }
    }
}
