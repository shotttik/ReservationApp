using Shared;
using System.ComponentModel.DataAnnotations;
namespace Application.Common.Requests.Admin
{
    public class BranchCreateRequest()
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