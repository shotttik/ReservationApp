using Shared;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class CompanyPartialUpdateRequest
    {
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public LocationUpdateRequest? Location { get; set; }

    }

    public class LocationUpdateRequest()
    {
        [MaxLength(255)]
        public string? AddressLine1 { get; set; }
        [MaxLength(255)]
        public string? AddressLine2 { get; set; }
        [MaxLength(255)]
        public string? City { get; set; }
        [MaxLength(20)]
        public string? PostalCode { get; set; }
        [MaxLength(255)]
        public string? State { get; set; }
        [DecimalPrecision(10, 8, ErrorMessage = "Latitude must have a precision of 9 and scale of 6.")]
        public decimal? Latitude { get; set; }
        [DecimalPrecision(11, 8, ErrorMessage = "Latitude must have a precision of 9 and scale of 6.")]
        public decimal? Longitude { get; set; }
    }
}
