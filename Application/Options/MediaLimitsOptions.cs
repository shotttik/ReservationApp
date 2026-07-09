namespace Application.Options
{
    public class MediaLimitsOptions
    {
        public const string ConfigurationSection = "MediaLimits";
        public ImageOptions Image { get; set; } = new();

        public class ImageOptions
        {
            public string [] AllowedFileTypes { get; set; } = [];
            public long MaxFileSizeInBytes { get; set; }
        }
    }
}
