using Application.Common.Requests.Admin;
using Domain.DTO.Company;
using Domain.Entities.CompanyReleated;

namespace Application.Extensions.Mappers
{
    public static class CompanyMapper
    {
        public static Domain.DTO.Company.CompanyDTO MapToDTO(this Company company)
        {
            return new Domain.DTO.Company.CompanyDTO
            {
                ID = company.ID,
                Name = company.Name,
                Description = company.Description,
                IN = company.IN,
                Email = company.Email,
                Phone = company.Phone,
                Type = company.Type,
                IsActive = company.IsActive,
                CreatedAt = company.CreatedAt,
                Services = company.Services.Select(s => new ServiceDTO
                {
                    ID = s.ID,
                    Name = s.Name,
                    Description = s.Description,
                    Duration = s.Duration,
                    Price = s.Price,
                    IsActive = s.IsActive,
                }).ToList()
            };
        }
        public static Company MapToEntity(this CompanyCreateRequest request) => new()
        {
            Name = request.Name,
            Description = request.Description,
            IN = request.IN,
            Email = request.Email,
            Phone = request.Phone,
            Type = request.Type,
            IsActive = request.IsActive,
            Location = request.Location.MapToEntity()
        };

        public static Company MapToEntity(this Domain.DTO.Company.CompanyDTO companyDTO)
        {
            return new Domain.Entities.CompanyReleated.Company
            {
                ID = companyDTO.ID,
                Name = companyDTO.Name,
                Description = companyDTO.Description,
                IN = companyDTO.IN,
                Email = companyDTO.Email,
                Phone = companyDTO.Phone,
                Type = companyDTO.Type,
                IsActive = companyDTO.IsActive,
                CreatedAt = companyDTO.CreatedAt
            };
        }

    }
}
