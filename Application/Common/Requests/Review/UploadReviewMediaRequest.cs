using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Review
{
    public class UploadReviewMediaRequest
    {
        [Required]
        public List<IFormFile> Media { get; set; } = [];
    }
}
