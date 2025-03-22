using Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Admin
{
    public class AddUserRequest
    {
        [Required(AllowEmptyStrings =false)]
        public required string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public required string LastName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public Gender Gender { get; set; }
        [Required(AllowEmptyStrings = false)]
        public DateTime? DateOfBirth { get; set; }
        [Required(AllowEmptyStrings = false)]
        public required string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        public required string Password { get; set; }
        [Required(AllowEmptyStrings = false)]
        public required int Role { get; set; }
    }
}
