using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class UploadCompanyMediasRequest
    {
        [Required]
        public required List<IFormFile> Medias { get; set; } = [];
    }
}
