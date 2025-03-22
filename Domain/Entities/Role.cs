using Shared.Enumerations;

namespace Domain.Entities
{
    public sealed class Role :Enumeration<Role>
    {
        public static Role SuperAdmin = new(1, nameof(SuperAdmin));
        public static Role Admin = new(2, nameof(Admin));
        public static Role User = new(3, nameof(User));
        public static Role CompanyAdmin = new(4, nameof(CompanyAdmin));
        public static Role CompanyMember = new(5, nameof(CompanyMember));

        public Role(int id, string name)
            : base(id, name)
        {
        }
        private Role() : base(default, default!) { }

        public ICollection<Permission> Permissions { get; set; } = [];
        public ICollection<UserAccount> UserAccounts { get; set; } = [];
    }
}
