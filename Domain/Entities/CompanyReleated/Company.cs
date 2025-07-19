using Domain.Entities.Common;
using Domain.Entities.LocationReleated;
using Domain.Entities.User;
using Domain.Enums;

namespace Domain.Entities.CompanyReleated
{
    public class Company :ActivableEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string IN { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public CompanyType Type { get; set; }
        public int LocationID { get; set; }
        public Location Location { get; set; } = null!;
        public ICollection<UserAccount> UserAccounts { get; set; } = [];
        public ICollection<CompanyInvitation> Invitations { get; set; } = [];
        public ICollection<Service> Services { get; set; } = [];
        public ICollection<Booking> Bookings { get; set; } = [];
        public ICollection<CompanyFAQCategory> CompanyFAQCategories { get; set; } = [];
        public ICollection<CompanyMedia> CompanyMedias { get; set; } = [];
    }
}
