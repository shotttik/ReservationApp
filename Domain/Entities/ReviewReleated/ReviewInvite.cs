using Domain.Entities.Common;

namespace Domain.Entities.ReviewReleated
{
    public class ReviewInvite :BaseEntity
    {
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; } = null!;
        public virtual Review Review { get; set; } = null!;
        public bool ClientReviewed { get; set; } = false;
        public DateTimeOffset OpenAt { get; set; }       // e.g., complete date 00:00 UTC
        public DateTimeOffset CloseAt { get; set; }      // +14 days
    }

}
