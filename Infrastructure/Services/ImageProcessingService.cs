using Domain.Interfaces.Services;
using SixLabors.ImageSharp.Formats.Webp;

namespace Infrastructure.Services
{
    public class ImageProcessingService :IImageProcessingService
    {
        public async Task<Stream> ConvertToWebp(Stream inputStream, CancellationToken cancellationToken, int maxWidth = 1024)
        {
            inputStream.Position = 0;
            using var image = await Image.LoadAsync(inputStream, cancellationToken);
            if (image.Width > maxWidth)
            {
                var ratio = (double)maxWidth / image.Width;
                image.Mutate(x => x.Resize(
                    new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(maxWidth, 0)
                    }));
            }

            var outputStream = new MemoryStream();

            var encoder = new WebpEncoder
            {
                Quality = 75
            };

            await image.SaveAsync(outputStream, encoder, cancellationToken);
            outputStream.Position = 0;

            return outputStream;
        }
    }
}
