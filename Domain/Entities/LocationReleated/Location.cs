using Domain.Entities.Common;
using Domain.Entities.CompanyReleated;

namespace Domain.Entities.LocationReleated
{
    public class Location :BaseEntity
    {
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public required string City { get; set; }
        public string? PostalCode { get; set; }
        public required string Country { get; set; }
        public Company? Company { get; set; }
    }
}
