using Domain.DTO.Company;

namespace Application.Extensions.Mappers
{
    public static class CompanyMapper
    {
        public static Domain.DTO.Company.CompanyDTO MapToDTO(this Domain.Entities.Company company)
        {
            return new Domain.DTO.Company.CompanyDTO
            {
                ID = company.ID,
                Name = company.Name,
                Description = company.Description,
                IN = company.IN,
                Email = company.Email,
                Phone = company.Phone,
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
        public static Domain.Entities.Company MapToEntity(this Domain.DTO.Company.CompanyDTO companyDTO)
        {
            return new Domain.Entities.Company
            {
                ID = companyDTO.ID,
                Name = companyDTO.Name,
                Description = companyDTO.Description,
                IN = companyDTO.IN,
                Email = companyDTO.Email,
                Phone = companyDTO.Phone,
                IsActive = companyDTO.IsActive,
                CreatedAt = companyDTO.CreatedAt
            };
        }

    }
}
