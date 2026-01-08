using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class BookingGuestInfoDTO
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        [Required]
        public VerificationType ContactType { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Contact { get; set; }
        [MaxLength(100)]
        public string? DisplayName { get; set; }
    }
}
