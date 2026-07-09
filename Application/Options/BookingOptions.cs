using System.ComponentModel.DataAnnotations;

namespace Application.Options
{
    public class BookingOptions
    {
        public const string ConfigurationSection = "BookingSettings";
        [Required]
        public int VerificationCodeLength { get; set; }
        [Required]
        public int VerificationCodeExpirationMinutes { get; set; }

        [Required]
        public GuestToken GuestToken { get; set; } = null!;
    }

    public class GuestToken
    {
        [Required]
        public string Key { get; set; } = null!;
        [Required]
        public int ExpirationMinutes { get; set; }
    }
}
