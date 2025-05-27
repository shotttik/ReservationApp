namespace Domain.DTO.User
{
    public class RoleDTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public List<PermissionDTO> Permissions { get; set; } = [];
    }
}