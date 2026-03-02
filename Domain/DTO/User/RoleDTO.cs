namespace Domain.DTO.User
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<PermissionDTO> Permissions { get; set; } = [];
    }
}