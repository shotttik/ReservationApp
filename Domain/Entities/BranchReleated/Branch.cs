using Domain.Entities.Common;
using Domain.Entities.CompanyReleated;
using Domain.Entities.User;

namespace Domain.Entities.BranchReleated
{
    public class Branch :ActivableEntity
    {
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public required string City { get; set; }
        public string? PostalCode { get; set; }
        public required string Country { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? State { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;
        public ICollection<UserAccount> UserAccounts { get; set; } = [];
        public ICollection<Booking> Bookings { get; set; } = [];
    }
}
