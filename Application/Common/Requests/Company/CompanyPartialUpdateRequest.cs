using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class CompanyPartialUpdateRequest
    {
        [Required]
        public required string Description { get; set; }

    }
}
