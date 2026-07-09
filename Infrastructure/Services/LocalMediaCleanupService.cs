using Application.Jobs;
using Application.Options;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Constants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class LocalMediaCleanupService :IMediaCleanupService
    {

        private readonly IMediaRepository _mediaRepository;
        private readonly ILogger<MediaCleanupJob> _logger;
        private readonly MediaCleanupJobOptions _mediaCleanupJobOptions;
        public LocalMediaCleanupService(
            IMediaRepository mediaRepository,
            ILogger<MediaCleanupJob> logger,
            IOptions<MediaCleanupJobOptions> mediaCleanupJobOptions)
        {
            _mediaRepository = mediaRepository;
            _logger = logger;
            _mediaCleanupJobOptions = mediaCleanupJobOptions.Value;
        }
        public async Task CleanupOrphanedMediaAsync()
        {
            var dbRemoteUrls = await _mediaRepository.GetAllRemoteUrls();

            List<WebpFileInfo> orphanedWebps = new();
            foreach (var absoultePath in EnumerateAllWebpFiles())
            {
                string remoteUrl = ToWebpRemoteUrl(absoultePath);
                if (!dbRemoteUrls.Contains(remoteUrl))
                {
                    orphanedWebps.Add(new()
                    {
                        AbsolutePath = absoultePath,
                        RemoteUrl = remoteUrl
                    });
                }
            }
            foreach (var webp in orphanedWebps)
            {
                if (File.Exists(webp.AbsolutePath) && IsSafeToDelete(webp.AbsolutePath))
                {
                    try
                    {
                        if (_mediaCleanupJobOptions.DryRun)
                        {
                            _logger.LogInformation($"Would delete file: {webp.AbsolutePath}");
                        }
                        else
                        {
                            File.Delete(webp.AbsolutePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        continue;
                    }
                }
                foreach (var original in FindOriginalFiles(webp))
                {
                    if (!IsSafeToDelete(original))
                    {
                        continue;
                    }
                    try
                    {
                        if (_mediaCleanupJobOptions.DryRun)
                        {
                            _logger.LogInformation($"Would delete file: {original}");
                        }
                        else
                        {
                            File.Delete(original);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        continue;
                    }
                }

            }
            IEnumerable<string> EnumerateAllWebpFiles()
            {
                foreach (var file in Directory.EnumerateFiles(
                    LocalMediaFolders.WebpUploadsRoot,
                    "*.webp",
                    SearchOption.AllDirectories))
                {
                    yield return file;
                }
            }
            IEnumerable<string> FindOriginalFiles(WebpFileInfo webp)
            {
                string webpPath = webp.AbsolutePath;

                // Extract base filename (abcd)
                string baseName = Path.GetFileNameWithoutExtension(webpPath);
                // Map webp → originals folder
                string originalsPath = webpPath
                    .Replace($"{Path.DirectorySeparatorChar}{LocalMediaFolders.WebpFolder}{Path.DirectorySeparatorChar}",
                             $"{Path.DirectorySeparatorChar}{LocalMediaFolders.OriginalsFolder}{Path.DirectorySeparatorChar}");
                string originalsFolder = Path.GetDirectoryName(originalsPath)!;

                if (!Directory.Exists(originalsFolder))
                    yield break;

                foreach (var file in Directory.EnumerateFiles(
                    originalsFolder,
                    baseName + ".*",
                    SearchOption.AllDirectories))
                {
                    yield return file;
                }

            }
            string ToWebpRemoteUrl(string absolutePath)
            {
                var directory = Path.GetDirectoryName(absolutePath)!
                    .Replace(LocalMediaFolders.WebpUploadsRoot, LocalMediaFolders.WebpUploadsRelative) // ar ikargeba ex. company-media folderi
                    .Replace("\\", "/"); // make URL-safe 

                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(absolutePath);
                return $"{directory}/{fileNameWithoutExt}.webp";
            }
            bool IsSafeToDelete(string filePath)
            {
                var fileInfo = new FileInfo(filePath);
                return fileInfo.CreationTimeUtc > DateTime.UtcNow.AddMinutes(-10) ? false : true;
            }
        }
        class WebpFileInfo
        {
            public string AbsolutePath { get; set; } = default!;
            public string RemoteUrl { get; set; } = default!;
        }
    }
}
