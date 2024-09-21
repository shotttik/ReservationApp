namespace Domain.Entities
{
    public class UserRole
    {
        public required int ID { get; set; }
        public required string RoleDescription { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Permission> Permissions { get; } = [];
        public ICollection<UserAccount> UserAccounts { get; } = [];
    }
}
