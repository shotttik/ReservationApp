using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Review
{
    public class UploadReviewMediasRequest
    {
        [Required]
        public List<IFormFile> Medias { get; set; } = [];
    }
}
