using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Company
{
    public class ServiceUpdateDTO:BaseServiceDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }
    }
}
