using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.User
{
    public class ChangeEmailRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public required string Email { get; set; }
    }
}
