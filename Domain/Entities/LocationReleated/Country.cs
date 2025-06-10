using Domain.Entities.Common;

namespace Domain.Entities.LocationReleated
{
    public class Country :BaseEntity
    {
        public required string Name { get; set; }
        public string? Iso3 { get; set; }
        public string? NumericCode { get; set; }
        public string? Iso2 { get; set; }
        public string? PhoneCode { get; set; }
        public string? Capital { get; set; }
        public string? Currency { get; set; }
        public string? CurrencyName { get; set; }
        public string? CurrencySymbol { get; set; }
        public string? Tld { get; set; }
        public string? Native { get; set; }
        public string? Region { get; set; }
        public int? RegionId { get; set; }
        public string? Subregion { get; set; }
        public int? SubregionId { get; set; }
        public string? Nationality { get; set; }
        public string? Timezones { get; set; }
        public string? Translations { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Emoji { get; set; }
        public string? EmojiU { get; set; }
        public bool Flag { get; set; } = true;
        public string? WikiDataId { get; set; }

        public ICollection<City> Cities { get; set; } = [];
    }
}
