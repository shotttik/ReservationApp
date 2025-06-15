using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.Company
{
    public class ServiceDTO :BaseServiceDTO
    {
        public int ID { get; set; }
    }
    public class BaseServiceDTO
    {
        [Required(AllowEmptyStrings = false)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public required int Duration { get; set; }
        [Required]
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
