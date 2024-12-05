using Application.Enums;
using Domain.Entities;

namespace Application.DTOs.Admin
{
    public class AddUserRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required HashSet<int> Roles { get; set; }
    }
}
