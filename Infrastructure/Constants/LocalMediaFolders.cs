namespace Infrastructure.Constants
{
    public static class LocalMediaFolders
    {
        public const string UploadsFolder = "uploads";
        public const string OriginalsFolder = "originals";
        public const string WebpFolder = "webp";
        public static string OriginalUploadsRelative => Path.Combine(UploadsFolder, OriginalsFolder);
        public static string OriginalUploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), OriginalUploadsRelative);

        public static string WebpUploadsRelative => Path.Combine(UploadsFolder, WebpFolder);
        public static string WebpUploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), WebpUploadsRelative);

    }
}
