using Application.Enums;

namespace Application.DTOs.Admin
{
    public class UpdateUserRequest
    {
        public int UserAccountID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public HashSet<int>? Roles { get; set; }
    }
}
