using Domain.Entities.Common;

namespace Domain.Entities.LocationReleated
{
    public class City :BaseEntity
    {
        public required string Name { get; set; }
        public int StateId { get; set; }
        public required string StateCode { get; set; }
        public required string StateName { get; set; }
        public int CountryId { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool Flag { get; set; } = true;
        public string? WikiDataId { get; set; }

        public Country Country { get; set; } = null!;
    }
}
