using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserLoginData
    {
        public int ID { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int HashAlgorithmID { get; set; }
        public string? ConfirmationToken { get; set; }
        public int UserAccountID { get; set; }
        public DateTime? TokenGenerationTime { get; set; }
        public int EmailValidationStatus { get; set; }
        public string? PasswordRecoveryToken { get; set; }
        public DateTime? RecoveryTokenTime { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public UserAccount UserAccount { get; set; } = null!;
        public HashingAlgorithm HashingAlgorithm { get; set; } = null!;
    }
}
