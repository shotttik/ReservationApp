using Domain.Enums;

namespace Domain.Entities.Common
{
    public class BookingGuestInfo :BaseEntity
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;
        public VerificationType ContactType { get; set; }
        public string Contact { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
    }
}
