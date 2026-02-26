using Domain.DTO;
using Domain.Entities.CompanyReleated;

namespace Application.Extensions.Mappers
{
    public static class CompanySubscriptionMapper
    {
        public static CompanySubscriptionDTO MapToDTO(this CompanySubscription entity)
        {
            return new CompanySubscriptionDTO
            {
                Id = entity.ID,
                CompanyId = entity.CompanyId,
                SubscriptionPlanId = entity.SubscriptionPlanId,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Status = entity.Status,
                AutoRenew = entity.AutoRenew,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}