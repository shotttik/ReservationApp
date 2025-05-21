using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.User
{
    public record ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public required string Email { get; set; }

    }
}
