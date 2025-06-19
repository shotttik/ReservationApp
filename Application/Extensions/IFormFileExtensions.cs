using Application.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Extensions
{
    public static class IFormFileExtensions
    {
        public static Error IsValidImage(this IFormFile file, IConfiguration configuration)
        {
            if (file == null || file.Length == 0)
                return MediaResults.InvalidFile;

            var contentType = file.ContentType.ToLowerInvariant();
            var allowedTypes = configuration.GetSection("MediaLimits:Image:AllowedFileTypes").Get<string []>();
            var maxSize = configuration.GetValue<long>("MediaLimits:Image:MaxFileSizeInBytes");
            if (allowedTypes == null || !allowedTypes.Contains(contentType))
                return MediaResults.InvalidImageType;
            if (file.Length > maxSize)
                return MediaResults.ImageTooLarge;

            return Error.None;
        }
    }
}
