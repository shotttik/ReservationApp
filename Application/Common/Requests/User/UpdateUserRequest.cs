using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.User
{
    public class UpdateUserRequest
    {
        [MaxLength(100)]
        [Required]
        public required string FirstName { get; set; }
        [MaxLength(200)]
        [Required]
        public required string LastName { get; set; }
        [Required]
        public Gender? Gender { get; set; }
        [Required]
        public DateOnly? DateOfBirth { get; set; }
    }
}
