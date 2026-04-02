using Domain.Enums;

namespace Domain.DTO.Branch
{
    public class BranchDTO
    {
        public int Id { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public required string City { get; set; }
        public string? PostalCode { get; set; }
        public required string Country { get; set; }
        public string? State { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
        public bool IsMain { get; set; }
    }
}
