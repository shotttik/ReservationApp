using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Company
{
    public class ServicesRequest<T> where T : BaseServiceDTO
    {
        [Required]
        [MinLength(1, ErrorMessage = "The Services list must contain at least one item.")]
        public List<T> Services { get; set; } = new();
    }
    public class CreateServicesRequest :ServicesRequest<CreateServiceDTO>
    {
    }
    public class UpdateServicesRequest :ServicesRequest<UpdateServiceDTO>
    {
    }
}
