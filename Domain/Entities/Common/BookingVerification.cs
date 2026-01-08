using Domain.Enums;

namespace Domain.Entities.Common
{
    public class BookingVerification :BaseEntity
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;
        public VerificationType VerificationType { get; set; }
        public string CodeHash { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
    }
}
