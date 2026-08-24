using Domain.Enums;

namespace Domain.Entities.Common
{
    public sealed class BookingHistory :BaseEntity
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;
        public ActionType ActionType { get; set; }
        public int? ChangedByUserId { get; set; }
        public BookingChangeSource Source { get; set; }
        public string ChangesJson { get; set; } = "[]";
    }
}
