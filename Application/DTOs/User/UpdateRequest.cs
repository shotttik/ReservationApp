using Application.Enums;

namespace Application.DTOs.User
{
    public class UpdateRequest
    {
        public int UserAccountID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserRole? Role { get; set; }
    }
}
