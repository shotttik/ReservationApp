using Domain.Enums;

namespace Domain.Entities
{
    public class UserLoginData :BaseEntity
    {
        public required string Email { get; set; }
        public required byte [] PasswordHash { get; set; }
        public required byte [] PasswordSalt { get; set; }
        public string? ConfirmationToken { get; set; }
        public int UserAccountID { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpTime { get; set; }
        public VerificationStatus VerificationStatus { get; set; }
        public string? RecoveryToken { get; set; }
        public DateTime? RecoveryTokenExpTime { get; set; }
        public string? PendingNewEmail { get; set; }
        public UserAccount UserAccount { get; set; } = null!;
    }
}
