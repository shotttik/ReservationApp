namespace Application.Options
{
    public class AppUrls
    {
        public const string ConfigurationSection = "AppUrls";

        public required string FrontendBaseUrl { get; set; }
        public required string ApiBaseUrl { get; set; }
        public required string EmailVerificationPath { get; set; }
        public string VerificationLink => ApiBaseUrl + EmailVerificationPath;
    }

}
