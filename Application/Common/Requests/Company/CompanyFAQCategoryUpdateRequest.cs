using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class CompanyFAQCategoryUpdateRequest :CompanyFAQCategoryCreateRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int Id { get; set; }
    }
}
