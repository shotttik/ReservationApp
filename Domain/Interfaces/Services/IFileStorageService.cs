using Domain.Enums;

namespace Domain.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<string> Upload(
            Stream fileStream, 
            string fileName,
            string contentType, 
            UploadSubFolder subFolder,
            CancellationToken cancellationToken); // subFolder = e.g. "company", "profile", "review"
        Task<(string OriginalPath, string WebpPath)> UploadWithWebp(
            Stream fileStream,
            string fileName, 
            string contentType,
            UploadSubFolder subFolder,
            CancellationToken cancellationToken);
        void Delete(string filePath);
    }
}
