using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities.ReviewReleated
{
    public class Review :BaseEntity
    {
        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;

        // star ratings (example for stays)
        public int Overall { get; set; }                           // 1..5
        public int? Cleanliness { get; set; }
        public int? Accuracy { get; set; }
        public int? CheckIn { get; set; }
        public int? Communication { get; set; }
        public int? Location { get; set; }
        public int? Value { get; set; }

        public string? Body { get; set; }
        public string? Locale { get; set; }
        public DateTimeOffset? PublishedAt { get; set; }
        public int ReviewInviteId { get; set; }
        public ReviewInvite ReviewInvite { get; set; } = null!;
        public ICollection<ReviewMedia> Media { get; set; } = new List<ReviewMedia>();
    }
}
