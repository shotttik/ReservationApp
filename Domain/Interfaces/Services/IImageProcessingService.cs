namespace Domain.Interfaces.Services
{
    public interface IImageProcessingService
    {
        Task<Stream> ConvertToWebp(Stream inputStream, int maxWidth = 1024);
    }
}
