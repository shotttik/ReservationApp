using System.ComponentModel.DataAnnotations;

namespace Application.Options
{
    public class CompanyOptions
    {
        public const string ConfigurationSection = "CompanyLimits";
        [Required]
        public int FAQLimitPerCategory { get; set; }

        [Required]
        public int MaxFAQCategories { get; set; }
    }
}
