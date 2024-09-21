namespace Domain.Entities
{
    public class Permission
    {
        public required int ID { get; set; }
        public required string PermissionDescription { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<UserRole>? UserRoles { get; } = [];
    }
}
