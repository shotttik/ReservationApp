using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.SubscriptionPlan
{
    public class SubscriptionPlanUpdateRequest
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = null!;
        [Required]
        public decimal PriceMonthly { get; set; }
        [Required]
        public int MaxEmployees { get; set; }
        [Required]
        public int MaxBookingsPerMonth { get; set; }
        [Required]
        public int MaxBranches { get; set; }
    }
}
