namespace Application.Options
{
    public class MediaCleanupJobSettings
    {
        public const string ConfigurationSection = "Quartz:MediaCleanupJobSettings";
        public string Cron { get; set; } = null!;
        public bool DryRun { get; set; } = false;
    }
}
