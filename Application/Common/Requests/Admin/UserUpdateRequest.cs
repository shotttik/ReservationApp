using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Admin
{
    public class UserUpdateRequest
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(200)]
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public Role? Role { get; set; }
    }
}
