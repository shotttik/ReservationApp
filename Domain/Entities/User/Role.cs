using Shared.Enumerations;

namespace Domain.Entities.User
{
    public sealed class Role :Enumeration<Role>
    {
        public static Role SuperAdmin = new(1, nameof(SuperAdmin));
        public static Role PublicUser = new(2, nameof(PublicUser));
        public static Role CompanyAdmin = new(3, nameof(CompanyAdmin));
        public static Role CompanyMember = new(4, nameof(CompanyMember));

        public Role(int id, string name)
            : base(id, name)
        {
        }
        private Role() : base(default, default!) { }

        public ICollection<Permission> Permissions { get; set; } = [];
        public ICollection<UserAccount> UserAccounts { get; set; } = [];
    }
}
