using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Admin
{
    public class UpdateUserRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int UserAccountID { get; set; }
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(200)]
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public int? RoleID { get; set; }
    }
}
