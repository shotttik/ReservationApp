using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.User
{
    public class UploadUserProfileImageRequest
    {
        [Required]
        public required IFormFile File { get; set; }
    }
}
