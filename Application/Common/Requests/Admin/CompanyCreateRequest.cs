using Domain.Enums;
using Shared;
using System.ComponentModel.DataAnnotations;
namespace Application.Common.Requests.Admin
{
    public class CompanyCreateRequest()
    {
        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }
        [MaxLength(4000)]
        public string? Description { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "IN must be only numbers.")]
        [Length(1, 30)]
        public required string IN { get; set; }
        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }
        [Length(1, 20)]
        public string? Phone { get; set; }
        [Required]
        public CompanyType Type { get; set; }
        public ActiveStatus ActiveStatus { get; set; } = ActiveStatus.Active;
        public required LocationCreateRequest Location { get; set; }
    }

    public class LocationCreateRequest()
    {
        [Required]
        [MaxLength(255)]
        public required string AddressLine1 { get; set; }
        [MaxLength(255)]
        public string? AddressLine2 { get; set; }
        [Required]
        [MaxLength(255)]
        public required string City { get; set; }
        [MaxLength(20)]
        public string? PostalCode { get; set; }
        [Required]
        [MaxLength(100)]
        public required string Country { get; set; }
        [MaxLength(255)]
        public string? State { get; set; }
        [DecimalPrecision(20, 15, ErrorMessage = "Latitude must have a precision of 20 and scale of 15.")]
        public decimal? Latitude { get; set; }
        [DecimalPrecision(20, 15, ErrorMessage = "Longitude must have a precision of 20 and scale of 15.")]
        public decimal? Longitude { get; set; }
    }

}