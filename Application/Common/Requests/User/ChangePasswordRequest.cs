using Application.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.User
{
    public class ChangePasswordRequest : IValidatableObject
    {
        [Required]
        [DataType(DataType.Password)]
        [PasswordComplexity]
        [MaxLength(255)]
        public required string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [PasswordComplexity]
        [MaxLength(255)]

        public required string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [MaxLength(255)]
        public required string ConfirmPassword { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CurrentPassword == Password)
            {
                yield return new ValidationResult(
                    "The current password and new password cannot be the same.",
                    new [] { nameof(CurrentPassword), nameof(Password) });
            }
        }

    }
}
