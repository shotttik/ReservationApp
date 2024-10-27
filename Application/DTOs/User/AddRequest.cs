using Application.Enums;

namespace Application.DTOs.User
{
    public class AddRequest
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required UserRole Role { get; set; }
    }
}
