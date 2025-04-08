namespace Application.DTOs.User
{
    public class UserAccountDTO
    {
        public int ID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public required RoleDTO Role { get; set; }
        public CompanyDTO? Company { get; set; }
    }

    public class RoleDTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public List<PermissionDTO> Permissions { get; set; } = [];
    }
    public class PermissionDTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
    }
    public class CompanyDTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string IN { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}