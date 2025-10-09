using Domain.Entities.Common;

namespace Domain.Entities.ReviewReleated
{
    public class ReviewMedia
    {
        public int ReviewId { get; set; }
        public int MediaId { get; set; }
        public virtual Review Review { get; set; } = default!;
        public virtual Media Media { get; set; } = default!;
    }
}
