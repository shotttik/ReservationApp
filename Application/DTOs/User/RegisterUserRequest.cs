using Application.Attributes;
using System.ComponentModel.DataAnnotations;
namespace Application.DTOs.User
{
    public class RegisterUserRequest
    {
        [Required(AllowEmptyStrings = false)]
        public required string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public required string LastName { get; set; }
        [Required]
        public int? Gender { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [EmailAddress]
        [Required(AllowEmptyStrings = false)]
        public required string Email { get; set; }
        [PasswordComplexity]
        [Required(AllowEmptyStrings = false)]
        public required string Password { get; set; }
        [Required]
        public required int RoleID { get; set; }
    }
}