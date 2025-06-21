using Domain.Enums;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using NUlid;

namespace Infrastructure.Services
{
    public class LocalFileStorageService :IFileStorageService
    {
        private readonly IImageProcessingService imageProcessingService;
        private readonly string OriginalUploadsRoot;
        private readonly string WebpUploadsRoot;
        private const string UploadsFolder = "uploads";
        private const string OriginalsFolder = "originals";
        private const string WebpFolder = "webp";
        private readonly string CurrentDirectory;


        public LocalFileStorageService(IImageProcessingService imageProcessingService)
        {
            CurrentDirectory = Directory.GetCurrentDirectory();
            this.imageProcessingService = imageProcessingService;

            OriginalUploadsRoot = Path.Combine(CurrentDirectory, UploadsFolder, OriginalsFolder);
            WebpUploadsRoot = Path.Combine(CurrentDirectory, UploadsFolder, WebpFolder);

            var uploadsRoot = Path.Combine(CurrentDirectory, UploadsFolder);
            EnsureDirectoryExists(uploadsRoot);
            EnsureDirectoryExists(OriginalUploadsRoot);
            EnsureDirectoryExists(WebpUploadsRoot);

            foreach (UploadSubFolder folder in Enum.GetValues(typeof(UploadSubFolder)))
            {
                EnsureDirectoryExists(Path.Combine(OriginalUploadsRoot, folder.GetFolderName()));
                EnsureDirectoryExists(Path.Combine(WebpUploadsRoot, folder.GetFolderName()));
            }
        }
        private static readonly HashSet<string> AllowedContentTypes = new()
            {
                "image/jpeg",
                "image/png"
            };

        public async Task<(string OriginalPath, string WebpPath)> UploadWithWebp(
            Stream fileStream,
            string fileName,
            string contentType,
            UploadSubFolder subFolder,
            CancellationToken cancellationToken)
        {
            if (!AllowedContentTypes.Contains(contentType.ToLower()))
                throw new InvalidOperationException("Unsupported file type");

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
            var folderPath = Path.Combine(OriginalUploadsRoot, folderName);

            var uniqueFileName = $"{Ulid.NewUlid()}{Path.GetExtension(fileName)}";
            var filePath = Path.Combine(folderPath, uniqueFileName);

            await using var writeStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await fileStream.CopyToAsync(writeStream, cancellationToken);

            var relativePath = Path.Combine(UploadsFolder, OriginalsFolder, folderName, uniqueFileName).Replace("\\", "/");

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
            var webpFolderPath = Path.Combine(WebpUploadsRoot, folderName);

            var baseName = Path.GetFileNameWithoutExtension(originalFilePath);
            var webpFileName = $"{baseName}.webp";
            var webpFilePath = Path.Combine(webpFolderPath, webpFileName);

            var convertedStream = await imageProcessingService.ConvertToWebp(fileStream, cancellationToken);

            await using var webpFileStreamToWrite = new FileStream(webpFilePath, FileMode.Create, FileAccess.Write);
            await convertedStream.CopyToAsync(webpFileStreamToWrite, cancellationToken);

            var webpRelativePath = Path.Combine(UploadsFolder, WebpFolder, folderName, webpFileName).Replace("\\", "/");

            return webpRelativePath;
        }

        public void Delete(string filePath)
        {
            var normalizedPath = filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(CurrentDirectory, normalizedPath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
