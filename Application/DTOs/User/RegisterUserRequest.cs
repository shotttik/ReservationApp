using Application.Attributes;
using System.ComponentModel.DataAnnotations;
namespace Application.DTOs.User
{
    public class RegisterUserRequest
    {
        [Required]
        [MaxLength(100)]
        public required string FirstName { get; set; }
        [Required]
        [MaxLength(200)]
        public required string LastName { get; set; }
        [Required]
        public int? Gender { get; set; }
        [Required]
        public DateOnly? DateOfBirth { get; set; }
        [EmailAddress]
        [Required]
        [MaxLength(255)]
        public required string Email { get; set; }
        [PasswordComplexity]
        [Required]
        [MaxLength(255)]
        public required string Password { get; set; }
        [Required]
        public required int RoleID { get; set; }
        public RegisterCompany? Company { get; set; }
    }

    public class RegisterCompany()
    {
        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "IN must be only numbers.")]
        [Length(9, 9)]
        public required string IN { get; set; }
        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }
        [Length(1, 30)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone must be only numbers.")]
        public string? Phone { get; set; }

    }
}