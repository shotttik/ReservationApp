using Microsoft.AspNetCore.Http;

namespace Application.Common.Requests.Company
{
    public class UpdateCompanyMediaRequest
    {
        public int MediaId { get; set; }

        public bool IsMain { get; set; }

        public bool IsNewImage { get; set; }

        public bool IsRemoved { get; set; }

        public IFormFile? File { get; set; }
    }
}
