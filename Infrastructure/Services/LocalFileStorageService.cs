using Domain.Enums;
using Domain.Interfaces.Services;
using Infrastructure.Constants;
using Infrastructure.Extensions;
using NUlid;

namespace Infrastructure.Services
{
    public class LocalFileStorageService :IFileStorageService
    {
        private readonly IImageProcessingService imageProcessingService;

        public LocalFileStorageService(IImageProcessingService imageProcessingService)
        {
            this.imageProcessingService = imageProcessingService;

            EnsureDirectoryExists(LocalMediaFolders.OriginalUploadsRoot);
            EnsureDirectoryExists(LocalMediaFolders.WebpUploadsRoot);

            foreach (UploadSubFolder folder in Enum.GetValues(typeof(UploadSubFolder)))
            {
                EnsureDirectoryExists(Path.Combine(LocalMediaFolders.OriginalUploadsRoot, folder.GetFolderName()));
                EnsureDirectoryExists(Path.Combine(LocalMediaFolders.WebpUploadsRoot, folder.GetFolderName()));
            }
        }

        public async Task<(string OriginalPath, string WebpPath)> UploadWithWebp(
            Stream fileStream,
            string fileName,
            string contentType,
            UploadSubFolder subFolder,
            CancellationToken cancellationToken)
        {

            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream, cancellationToken);

            var originalPath = await Upload(memoryStream, fileName, contentType, subFolder, cancellationToken);
            var webpPath = await UploadWebp(memoryStream, originalPath, subFolder, cancellationToken);

            return (originalPath, webpPath);
        }

        public async Task<string> Upload(
            Stream fileStream,
            string fileName,
            string contentType,
            UploadSubFolder subFolder,
            CancellationToken cancellationToken)
        {
            fileStream.Position = 0;
            var folderName = subFolder.GetFolderName();
            var folderPath = Path.Combine(LocalMediaFolders.OriginalUploadsRoot, folderName);

            var uniqueFileName = $"{Ulid.NewUlid()}{Path.GetExtension(fileName)}";
            var filePath = Path.Combine(folderPath, uniqueFileName);

            await using var writeStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await fileStream.CopyToAsync(writeStream, cancellationToken);

            var relativePath = Path.Combine(LocalMediaFolders.OriginalUploadsRelative, folderName, uniqueFileName).Replace("\\", "/");

            return relativePath;
        }

        private async Task<string> UploadWebp(
            Stream fileStream,
            string originalFilePath,
            UploadSubFolder subFolder,
            CancellationToken cancellationToken)
        {
            fileStream.Position = 0;
            var folderName = subFolder.GetFolderName();
            var webpFolderPath = Path.Combine(LocalMediaFolders.WebpUploadsRoot, folderName);

            var baseName = Path.GetFileNameWithoutExtension(originalFilePath);
            var webpFileName = $"{baseName}.webp";
            var webpFilePath = Path.Combine(webpFolderPath, webpFileName);

            var convertedStream = await imageProcessingService.ConvertToWebp(fileStream, cancellationToken);

            await using var webpFileStreamToWrite = new FileStream(webpFilePath, FileMode.Create, FileAccess.Write);
            await convertedStream.CopyToAsync(webpFileStreamToWrite, cancellationToken);

            var webpRelativePath = Path.Combine(LocalMediaFolders.WebpUploadsRelative, folderName, webpFileName).Replace("\\", "/");

            return webpRelativePath;
        }

        public void Delete(string filePath)
        {
            var normalizedPath = filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), normalizedPath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        public void DeleteAll(IEnumerable<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                Delete(filePath);
            }
        }
        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
