namespace Domain.DTO.Review
{
    public class ReviewInviteDTO
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public bool ClientReviewed { get; set; } = false;
        public DateTimeOffset OpenAt { get; set; }       // e.g., complete date 00:00 UTC
        public DateTimeOffset CloseAt { get; set; }      // +14 days
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
