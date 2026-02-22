using Domain.Enums;

namespace Domain.DTO
{
    public class SubscriptionUsageDTO
    {
        // Subscription state
        public SubscriptionStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Plan limits
        public int MaxEmployees { get; set; }
        public int MaxBranches { get; set; }
        public int MaxBookingsPerMonth { get; set; }

        // Current usage
        public int EmployeeCount { get; set; }
        public int BranchCount { get; set; }
        public int MonthlyBookingCount { get; set; }
        public bool IsActive =>
            Status == SubscriptionStatus.Active &&
            EndDate > DateTime.UtcNow;
    }
}
