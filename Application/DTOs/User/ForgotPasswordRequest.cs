using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public record ForgotPasswordRequest
    {
        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public required string Email { get; set; }

    }
}
