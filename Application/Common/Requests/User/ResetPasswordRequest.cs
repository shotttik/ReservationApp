using Application.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.User
{
    public record ResetPasswordRequest
    {
        [Required]
        [DataType(DataType.Password)]
        [PasswordComplexity]
        [MaxLength(255)]
        public required string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [MaxLength(255)]
        public required string ConfirmPassword { get; set; }
        [Required(AllowEmptyStrings = false)]
        public required string RecoveryToken { get; set; }
    }
}
