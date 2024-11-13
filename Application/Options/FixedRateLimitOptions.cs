namespace Application.Options
{
    public class FixedRateLimitOptions
    {
        public const string FixedRateLimit = "FixedRateLimit";
        public int PermitLimit { get; set; } = 10;
        public int Window { get; set; } = 10;
        public int QueueLimit { get; set; } = 100;
    }
}
