using Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace Application.Common.Requests.Admin
{
    public class CompanyCreateRequest()
    {
        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }
        [MaxLength(4000)]
        public string? Description { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "IN must be only numbers.")]
        [Length(1, 30)]
        public required string IN { get; set; }
        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }
        [Length(1, 20)]
        public string? Phone { get; set; }
        [Required]
        public CompanyType Type { get; set; }
        public ActiveStatus ActiveStatus { get; set; } = ActiveStatus.Active;
    }
}