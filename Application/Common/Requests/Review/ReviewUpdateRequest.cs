using Domain.Enums;

namespace Application.Common.Requests.Review
{
    public class ReviewUpdateRequest
    {
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
    }
}
