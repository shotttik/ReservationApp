using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class CompanyFAQCreateRequest
    {
        [Required]
        [MaxLength(500)]
        public required string Question { get; set; }
        [Required]
        [MaxLength(2000)]
        public required string Answer { get; set; }
        public ActiveStatus ActiveStatus { get; set; } = ActiveStatus.Active;
        [Required]
        public int Order { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int CategoryId { get; set; }
    }
}
