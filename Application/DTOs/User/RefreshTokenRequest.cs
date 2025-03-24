using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class TokenRequest
    {
        [Required]
        public required string AccessToken { get; set; }

        [Required]
        public required string RefreshToken { get; set; }
    }
}
