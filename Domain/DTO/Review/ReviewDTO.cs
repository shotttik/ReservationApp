using Domain.DTO.Company;
using Domain.Enums;

namespace Domain.DTO.Review
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public ReviewStatus Status { get; set; }

        // star ratings (example for stays)
        public int Overall { get; set; }                           // 1..5
        public int Cleanliness { get; set; }
        public int Accuracy { get; set; }
        public int CheckIn { get; set; }
        public int Communication { get; set; }
        public int Location { get; set; }
        public int Value { get; set; }

        public string? Body { get; set; }
        public string? Locale { get; set; }
        public DateTimeOffset? PublishedAt { get; set; }
        public int ReviewInviteId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public IEnumerable<ReviewMediaDTO> Media { get; set; } = [];
    }
}
