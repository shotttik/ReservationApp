using Application.Common.Requests.Admin;
using Application.Common.Requests.Company;
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
                Id = company.ID,
                Name = company.Name,
                Description = company.Description,
                IN = company.IN,
                Email = company.Email,
                Phone = company.Phone,
                Type = company.Type,
                ActiveStatus = company.ActiveStatus,
                Branches = company.Branches.Select(e => e.MapToDTO()),
                CreatedAt = company.CreatedAt,
                Viewed = company.Viewed,
                Services = company.Services.Select(s => new ServiceDTO
                {
                    Id = s.ID,
                    Name = s.Name,
                    Description = s.Description,
                    Duration = s.Duration,
                    Price = s.Price,
                    ActiveStatus = s.ActiveStatus,
                }).ToList(),
                Media = company.CompanyMedia.Select(cm => new CompanyMediaDTO
                {
                    Id = cm.MediaID,
                    IsMain = cm.IsMain,
                    ImageUrlWebp = cm.Media.RemoteUrl,
                    ImageUrlOriginal = cm.Media.OriginalUrl
                }).ToList(),
                Subscription = company.Subscription?.MapToDTO()
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
            ActiveStatus = request.ActiveStatus
        };

        public static Company MapToEntity(this Domain.DTO.Company.CompanyDTO companyDTO)
        {
            return new Domain.Entities.CompanyReleated.Company
            {
                ID = companyDTO.Id,
                Name = companyDTO.Name,
                Description = companyDTO.Description,
                IN = companyDTO.IN,
                Email = companyDTO.Email,
                Phone = companyDTO.Phone,
                Type = companyDTO.Type,
                ActiveStatus = companyDTO.ActiveStatus,
                CreatedAt = companyDTO.CreatedAt
            };
        }
        public static Company MapToEntity(this CompanyUpdateRequest request, Company entity)
        {
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.IN = request.IN;
            entity.Email = request.Email;
            entity.Phone = request.Phone;
            entity.Type = request.Type;
            entity.ActiveStatus = request.ActiveStatus;

            return entity;
        }
        public static void ApplyPartialUpdate(this Company company, CompanyPartialUpdateRequest req)
        {
            if (!string.IsNullOrWhiteSpace(req.Description))
                company.Description = req.Description;

            if (!string.IsNullOrWhiteSpace(req.Phone))
                company.Phone = req.Phone;
        }
    }
}
