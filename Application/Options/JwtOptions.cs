using System.ComponentModel.DataAnnotations;

namespace Application.Options
{
    public class JwtOptions
    {
        public const string ConfigurationSection = "Jwt";
        [Required]
        public string Key { get; set; } = null!;
        [Required]
        public string Issuer { get; set; } = null!;
        [Required]
        public string Audience { get; set; } = null!;
        [Required]
        public int AccessTokenExpirationMinutes { get; set; }
        [Required]
        public int RefreshTokenExpirationDays { get; set; }
        [Required]
        public int RecoveryTokenExpirationMinutes { get; set; }
        [Required]
        public int VerificationTokenExpirationDays { get; set; }
        [Required]
        public int CompanyInvitationExpirationDays { get; set; }
        [Required]
        public int UserActiveSessionsExpirationDays { get; set; }
    }
}
