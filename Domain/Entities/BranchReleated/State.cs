using Domain.Entities.Common;

namespace Domain.Entities.BranchReleated
{
    public class State :BaseEntity
    {
        public required string Name { get; set; }
        public int CountryId { get; set; }
        public required string CountryCode { get; set; }
        public string? CountryName { get; set; }
        public string? StateCode { get; set; }
        public string? FipsCode { get; set; }
        public string? Iso2 { get; set; }
        public string? Type { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? WikiDataId { get; set; }
        public Country Country { get; set; } = null!;
        public ICollection<City> Cities { get; set; } = [];
    }
}
