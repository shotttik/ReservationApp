using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class CompanyFAQCategoryCreateRequest
    {
        [Required]
        [MaxLength(500)]
        public required string Name { get; set; }
        public int Order { get; set; }
    }
}
