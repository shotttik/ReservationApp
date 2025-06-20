namespace Domain.Interfaces.Services
{
    public interface IImageProcessingService
    {
        Task<Stream> ConvertToWebp(Stream inputStream, CancellationToken cancellationToken, int maxWidth = 1024);
    }
}
