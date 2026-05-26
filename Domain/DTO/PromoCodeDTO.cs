using System.Text.Json.Serialization;

namespace Domain.DTO
{
    public class PromoCodeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercent { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int? MaxUsage { get; set; }
        public int UsedCount { get; set; }
        public int CompanyId { get; set; }
        public int? MinBookingAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
