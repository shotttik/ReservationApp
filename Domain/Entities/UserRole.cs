namespace Domain.Entities
{
    public class UserRole
    {
        public int ID { get; set; }
        public string RoleDescription { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Permission> Permissions { get; } = [];
        public ICollection<UserAccount> UserAccounts { get; } = [];
    }
}
