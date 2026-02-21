using Domain.Entities.CompanyReleated;

namespace Domain.Entities.Common
{
    public class SubscriptionPlan :BaseEntity
    {
        public string Name { get; set; } = null!;
        public decimal PriceMonthly { get; set; }

        // Limits
        public int MaxEmployees { get; set; }
        public int MaxBookingsPerMonth { get; set; }
        public int MaxBranches { get; set; }

        public ICollection<CompanySubscription> CompanySubscriptions { get; set; } = [];
    }
}
