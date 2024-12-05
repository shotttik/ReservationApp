using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class TokenRequest
    {
        [Required(AllowEmptyStrings = false)]
        public required string AccessToken { get; set; }

        [Required(AllowEmptyStrings = false)]
        public required string RefreshToken { get; set; }
    }
}
