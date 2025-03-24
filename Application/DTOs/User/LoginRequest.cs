using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public required string Email { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Password { get; set; }
    }
}
