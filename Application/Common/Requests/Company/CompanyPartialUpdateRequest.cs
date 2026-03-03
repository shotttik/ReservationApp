using Domain.Enums;

namespace Application.Common.Requests.Company
{
    public class CompanyPartialUpdateRequest
    {
        public string? Description { get; set; }
        public string? Phone { get; set; }
    }
}
