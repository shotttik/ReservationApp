using Application.Common.Results;
using Application.Options;
using Microsoft.AspNetCore.Http;

namespace Application.Extensions
{
    public static class IFormFileExtensions
    {
        public static Error IsValidImage(this IFormFile file, MediaLimitsOptions mediaLimitsOptions)
        {
            if (file == null || file.Length == 0)
                return MediaResults.InvalidFile;

            var contentType = file.ContentType.ToLowerInvariant();
            var allowedTypes = mediaLimitsOptions.Image.AllowedFileTypes;
            var maxSize = mediaLimitsOptions.Image.MaxFileSizeInBytes;
            if (allowedTypes == null || !allowedTypes.Contains(contentType))
                return MediaResults.InvalidImageType;
            if (file.Length > maxSize)
                return MediaResults.ImageTooLarge;

            return Error.None;
        }
    }
}
