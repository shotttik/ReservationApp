using Application.Common.Requests.Company;
using Domain.DTO.Company;
using Domain.Entities.CompanyReleated;

namespace Application.Extensions.Mappers
{
    public static class CompanyFAQMapper
    {
        public static CompanyFAQ MapToEntity(this CompanyFAQCreateRequest request)
        {
            return new CompanyFAQ
            {
                Question = request.Question,
                Answer = request.Answer,
                ActiveStatus = request.ActiveStatus,
                Order = request.Order,
                CategoryID = request.CategoryId
            };
        }
        public static void MapToEntity(this CompanyFAQUpdateRequest request, CompanyFAQ companyFAQ)
        {
            companyFAQ.ID = request.Id;
            companyFAQ.Question = request.Question;
            companyFAQ.Answer = request.Answer;
            companyFAQ.ActiveStatus = request.ActiveStatus;
            companyFAQ.Order = request.Order;
            companyFAQ.CategoryID = request.CategoryId;
        }
        public static CompanyFAQDTO MapToDTO(this CompanyFAQ entity)
        {
            return new CompanyFAQDTO
            {
                Id = entity.ID,
                Question = entity.Question,
                Answer = entity.Answer,
                ActiveStatus = entity.ActiveStatus,
                Order = entity.Order,
                CategoryId = entity.CategoryID,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public static CompanyFAQCategory MapToEntity(this CompanyFAQCategoryCreateRequest request)
        {
            return new CompanyFAQCategory
            {
                Name = request.Name,
                Order = request.Order,
                ActiveStatus = request.ActiveStatus,
            };
        }
        public static void MapToEntity(this CompanyFAQCategoryUpdateRequest request, CompanyFAQCategory companyFAQ)
        {
            companyFAQ.Name = request.Name;
            companyFAQ.Order = request.Order;
            companyFAQ.ActiveStatus = request.ActiveStatus;
        }

        public static CompanyFAQCategoryDTO MapToDTO(this CompanyFAQCategory entity)
        {
            return new CompanyFAQCategoryDTO
            {
                Id = entity.ID,
                Name = entity.Name,
                Order = entity.Order,
                ActiveStatus = entity.ActiveStatus,
                CompanyId = entity.CompanyID,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
