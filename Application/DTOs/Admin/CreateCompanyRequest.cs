using System.ComponentModel.DataAnnotations;
namespace Application.DTOs.Admin
{
    public class CreateCompanyRequest()
    {
        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "IN must be only numbers.")]
        [Length(9, 9)]
        public required string IN { get; set; }
        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }
        [Length(1, 30)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone must be only numbers.")]
        public string? Phone { get; set; }
    }
}