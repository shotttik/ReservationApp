using Application.Attributes;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class CreateCompanyMemberRequest
    {
        [Required]
        [MaxLength(100)]
        public required string FirstName { get; set; }
        [Required]
        [MaxLength(200)]
        public required string LastName { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateOnly? DateOfBirth { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Email { get; set; }
        [Required]
        [PasswordComplexity]
        public required string Password { get; set; }
    }
}
