using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class UploadCompanyImagesRequest
    {
        [Required]
        public required List<ComapnyImages> Images { get; set; } = [];
    }
    public class ComapnyImages
    {
        [Required]
        public required IFormFile File { get; set; }
        [Required]
        public required bool IsMain { get; set; }
    }
}
