namespace Application.Common.Requests.Review
{
    public class ReviewCreateRequest
    {
        public int InviteId { get; set; }
        public int Cleanliness { get; set; }
        public int Accuracy { get; set; }
        public int CheckIn { get; set; }
        public int Communication { get; set; }
        public int Location { get; set; }
        public int Value { get; set; }

        public string? Body { get; set; }
        public string? Locale { get; set; }
        public List<int> MediaIds { get; set; } = [];
    }
}
