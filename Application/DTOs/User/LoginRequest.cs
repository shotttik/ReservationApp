using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class LoginRequest
    {
        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public required string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        public required string Password { get; set; }
    }
}
