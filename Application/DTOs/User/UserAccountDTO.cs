namespace Application.DTOs.User
{
    public class UserAccountDTO
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserRoleDTO Role { get; set; }

    }

    public class UserRoleDTO
    {
        public int ID { get; set; }
        public string RoleDescription { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }
    public class PermissionDTO
    {
        public int ID { get; set; }
        public string PermissionDescription { get; set; }
    }
}