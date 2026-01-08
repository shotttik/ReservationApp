using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Booking
{
    public class BookingGuestInfoCreateRequest
    {
        [Required]
        public VerificationType ContactType { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Contact { get; set; }
        [MaxLength(100)]
        public string? DisplayName { get; set; }
    };
}
