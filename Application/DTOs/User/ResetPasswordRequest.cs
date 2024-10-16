using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public record ResetPasswordRequest
    {
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }
        public required string Email { get; set; }
        public required string RecoveryToken { get; set; }
    }
}
