using Domain.Entities.CompanyReleated;

namespace Domain.Entities.Common
{
    public class PromoCode :ActivableEntity
    {
        public string Code { get; set; } = null!;

        public decimal? DiscountAmount { get; set; } // fixed
        public decimal? DiscountPercent { get; set; } // %

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public int? MaxUsage { get; set; }
        public int UsedCount { get; set; }

        public int CompanyId { get; set; }   // FK

        public Company Company { get; set; } = null!;

        // Optional constraints
        public int? MinBookingPrice { get; set; } //minimum required price to ALLOW promo usage It’s a restriction, not a calculation
        public ICollection<Booking> Bookings { get; set; } = []; // optional
    }
}
