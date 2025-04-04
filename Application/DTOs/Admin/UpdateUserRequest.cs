using Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Admin
{
    public class UpdateUserRequest
    {
        public int? UserAccountID { get; set; }
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(200)]
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public int? RoleID { get; set; }
    }
}
