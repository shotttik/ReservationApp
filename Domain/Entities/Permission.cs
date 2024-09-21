namespace Domain.Entities
{
    public class Permission
    {
        public int ID { get; set; }
        public string PermissionDescription { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<UserRole>? UserRoles { get; } = [];
    }
}
