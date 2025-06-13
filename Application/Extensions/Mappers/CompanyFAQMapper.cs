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
                IsActive = request.IsActive,
                Order = request.Order,
                CategoryID = request.CategoryID
            };
        }
        public static CompanyFAQ MapToEntity(this CompanyFAQUpdateRequest request)
        {
            return new CompanyFAQ
            {
                ID = request.ID,
                Question = request.Question,
                Answer = request.Answer,
                IsActive = request.IsActive,
                Order = request.Order,
                CategoryID = request.CategoryID
            };
        }
        public static void MapToEntity(this CompanyFAQUpdateRequest request, CompanyFAQ companyFAQ)
        {
            companyFAQ.Question = request.Question;
            companyFAQ.Answer = request.Answer;
            companyFAQ.IsActive = request.IsActive;
            companyFAQ.Order = request.Order;
            companyFAQ.CategoryID = request.CategoryID;
        }
        public static CompanyFAQDTO MapToDTO(this CompanyFAQ entity)
        {
            return new CompanyFAQDTO
            {
                ID = entity.ID,
                Question = entity.Question,
                Answer = entity.Answer,
                IsActive = entity.IsActive,
                Order = entity.Order,
                CategoryID = entity.CategoryID,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

        public static CompanyFAQCategory MapToEntity(this CompanyFAQCategoryCreateRequest request)
        {
            return new CompanyFAQCategory
            {
                Name = request.Name,
                Order = request.Order
            };
        }
        public static void MapToEntity(this CompanyFAQCategoryUpdateRequest request, CompanyFAQCategory companyFAQ)
        {
            companyFAQ.Name = request.Name;
            companyFAQ.Order = request.Order;
        }

        public static CompanyFAQCategoryDTO MapToDTO(this CompanyFAQCategory entity)
        {
            return new CompanyFAQCategoryDTO
            {
                ID = entity.ID,
                Name = entity.Name,
                Order = entity.Order,
                CompanyID = entity.CompanyID,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
