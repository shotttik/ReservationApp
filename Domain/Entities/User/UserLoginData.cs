using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Entities.User
{
    public class UserLoginData :ActivableEntity
    {
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public required byte [] PasswordHash { get; set; }
        public required byte [] PasswordSalt { get; set; }
        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationTokenExpTime { get; set; }
        public VerificationStatus EmailVerificationStatus { get; set; }
            = VerificationStatus.Unverified;
        public string? PhoneVerificationToken { get; set; }
        public DateTime? PhoneVerificationTokenExpTime { get; set; }
        public VerificationStatus PhoneVerificationStatus { get; set; }
            = VerificationStatus.Unverified;
        public string? RecoveryToken { get; set; }
        public DateTime? RecoveryTokenExpTime { get; set; }
        public string? PendingNewEmail { get; set; }
        public string? PendingNewPhone { get; set; }
        public virtual UserAccount UserAccount { get; set; } = null!;
        public bool IsEmailVerified => EmailVerificationStatus == VerificationStatus.Verified;
        public bool IsPhoneVerified => PhoneVerificationStatus == VerificationStatus.Verified;
    }
}
