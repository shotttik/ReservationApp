using Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Admin
{
    public class AddUserRequest
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
        public DateTime? DateOfBirth { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public required int Role { get; set; }
    }
}
