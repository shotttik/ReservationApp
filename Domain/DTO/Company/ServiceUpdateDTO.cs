using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.Company
{
    public class ServiceUpdateDTO :BaseServiceDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }
    }
}
