namespace Domain.DTO
{
    public class SubscriptionPlanDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal PriceMonthly { get; set; }
        public int MaxEmployees { get; set; }
        public int MaxBookingsPerMonth { get; set; }
        public int MaxBranches { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
