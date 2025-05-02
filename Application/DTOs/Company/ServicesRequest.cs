using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Company
{
    public class ServicesRequest<T> where T : BaseServiceDTO
    {
        [Required]
        public List<T> Services { get; set; } = new();
    }
    public class CreateServicesRequest :ServicesRequest<CreateServiceDTO>
    {
    }
    public class UpdateServicesRequest :ServicesRequest<UpdateServiceDTO>
    {
    }
}
