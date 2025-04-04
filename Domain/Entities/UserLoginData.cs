using Domain.Enums;

namespace Domain.Entities
{
    public class UserLoginData
    {
        public int ID { get; set; }
        public required string Email { get; set; }
        public required byte [] PasswordHash { get; set; }
        public required byte [] PasswordSalt { get; set; }
        public string? ConfirmationToken { get; set; }
        public int UserAccountID { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpTime { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpTime { get; set; }
        public VerificationStatus VerificationStatus { get; set; }
        public string? RecoveryToken { get; set; }
        public DateTime? RecoveryTokenExpTime { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }

        public UserAccount UserAccount { get; set; } = null!;
        public void UpdateTimestamp() => UpdatedAt = DateTime.Now;

    }
}
