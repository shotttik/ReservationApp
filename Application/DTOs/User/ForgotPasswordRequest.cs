using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public record ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public required string Email { get; set; }

    }
}
