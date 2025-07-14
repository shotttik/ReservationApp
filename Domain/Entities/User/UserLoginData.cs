using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities.User
{
    public class UserLoginData :ActivableEntity
    {
        public required string Email { get; set; }
        public required byte [] PasswordHash { get; set; }
        public required byte [] PasswordSalt { get; set; }
        public string? ConfirmationToken { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerificationTokenExpTime { get; set; }
        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Unverified;
        public string? RecoveryToken { get; set; }
        public DateTime? RecoveryTokenExpTime { get; set; }
        public string? PendingNewEmail { get; set; }
        public virtual UserAccount UserAccount { get; set; } = null!;

    }
}
