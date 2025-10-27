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
                ID = company.ID,
                Name = company.Name,
                Description = company.Description,
                IN = company.IN,
                Email = company.Email,
                Phone = company.Phone,
                Type = company.Type,
                ActiveStatus = company.ActiveStatus,
                Location = company.Location.MapToDTO(),
                CreatedAt = company.CreatedAt,
                Viewed = company.Viewed,
                Services = company.Services.Select(s => new ServiceDTO
                {
                    ID = s.ID,
                    Name = s.Name,
                    Description = s.Description,
                    Duration = s.Duration,
                    Price = s.Price,
                    ActiveStatus = s.ActiveStatus,
                }).ToList(),
                Medias = company.CompanyMedias.Select(cm => new MediaDTO
                {
                    ID = cm.MediaID,
                    IsMain = cm.IsMain,
                    Path = cm.Media.RemoteUrl
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
            ActiveStatus = request.ActiveStatus,
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
            entity.Location = request.Location.MapToEntity(entity.Location);

            return entity;
        }
        public static void ApplyPartialUpdate(this Company company, CompanyPartialUpdateRequest req)
        {
            if (!string.IsNullOrWhiteSpace(req.Description))
                company.Description = req.Description;

            if (!string.IsNullOrWhiteSpace(req.Phone))
                company.Phone = req.Phone;

            if (req.Location is not null)
            {
                var loc = req.Location;

                if (!string.IsNullOrWhiteSpace(loc.AddressLine1))
                    company.Location.AddressLine1 = loc.AddressLine1;

                if (!string.IsNullOrWhiteSpace(loc.AddressLine2))
                    company.Location.AddressLine2 = loc.AddressLine2;

                if (!string.IsNullOrWhiteSpace(loc.City))
                    company.Location.City = loc.City;

                if (!string.IsNullOrWhiteSpace(loc.State))
                    company.Location.State = loc.State;

                if (!string.IsNullOrWhiteSpace(loc.PostalCode))
                    company.Location.PostalCode = loc.PostalCode;

                if (loc.Latitude.HasValue)
                    company.Location.Latitude = loc.Latitude;

                if (loc.Longitude.HasValue)
                    company.Location.Longitude = loc.Longitude;
            }
        }

    }
}
