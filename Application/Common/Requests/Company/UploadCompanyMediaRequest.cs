using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class UploadCompanyMediaRequest
    {
        [Required]
        public required List<IFormFile> Media { get; set; } = [];
    }
}
