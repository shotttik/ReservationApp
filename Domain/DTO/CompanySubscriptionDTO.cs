using Domain.Enums;

namespace Domain.DTO
{
    public class CompanySubscriptionDTO
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int SubscriptionPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SubscriptionStatus Status { get; set; }

        public bool AutoRenew { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
