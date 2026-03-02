using Application.Common.Requests.Company;
using Domain.DTO.Company;
using Domain.Entities.CompanyReleated;

namespace Application.Extensions.Mappers
{
    public static class ServiceMapper
    {

        public static Service MapToEntity(this BaseServiceDTO service, int companyID)
        {
            return new Service
            {
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                Duration = service.Duration,
                ActiveStatus = service.ActiveStatus,
                CompanyID = companyID
            };
        }
        public static Service MapToEntity(this ServiceUpdateDTO service, int companyID)
        {
            return new Service
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                Duration = service.Duration,
                ActiveStatus = service.ActiveStatus,
                CompanyID = companyID
            };
        }
        public static ServiceDTO MapToDTO(this Service service)
        {
            return new ServiceDTO
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                Duration = service.Duration,
                ActiveStatus = service.ActiveStatus,
            };
        }
    }
}
