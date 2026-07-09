namespace Application.Options
{
    public class BookingOptions
    {
        public const string ConfigurationSection = "BookingSettings";
        public int VerificationCodeLength { get; set; }
        public int VerificationCodeExpirationMinutes { get; set; }

        public GuestToken GuestToken { get; set; } = null!;
    }

    public class GuestToken
    {
        public string Key { get; set; } = null!;
        public int ExpirationMinutes { get; set; }
    }
}
