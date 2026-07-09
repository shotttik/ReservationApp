using System.ComponentModel.DataAnnotations;

namespace Application.Options
{
    public class RedisOptions
    {
        public const string ConfigurationSection = "Redis";
        [Required]
        public int CacheExpirationMinutes { get; set; }
    }
}
