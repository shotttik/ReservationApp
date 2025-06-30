using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Admin
{
    public class CompanyUpdateRequest :CompanyCreateRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }
    }
}
