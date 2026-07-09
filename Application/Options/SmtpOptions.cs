namespace Application.Options
{
    public class SmtpOptions
    {
        public const string ConfigurationSection = "SmtpSettings";
        public required string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; } = "NoReply";
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
