using Domain.DTO.Company;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class ServicesRequest<T> where T : BaseServiceDTO
    {
        [Required]
        [MinLength(1, ErrorMessage = "The Services list must contain at least one item.")]
        public List<T> Services { get; set; } = new();
    }
    public class ServicesCreateRequest :ServicesRequest<ServiceCreateDTO>
    {
    }
    public class ServicesUpdateRequest :ServicesRequest<ServiceUpdateDTO>
    {
    }
    public class ServiceCreateDTO :BaseServiceDTO
    {
    }
    public class ServiceUpdateDTO :BaseServiceDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int Id { get; set; }
    }
}
