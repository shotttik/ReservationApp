using Domain.DTO;
using Domain.Entities.Common;

namespace Application.Extensions.Mappers
{
    public static class SubscriptionPlanMapper
    {
        public static SubscriptionPlanDTO MapToDTO(this SubscriptionPlan entity)
        {
            return new SubscriptionPlanDTO
            {
                Id = entity.ID,
                Name = entity.Name,
                PriceMonthly = entity.PriceMonthly,
                MaxEmployees = entity.MaxEmployees,
                MaxBookingsPerMonth = entity.MaxBookingsPerMonth,
                MaxBranches = entity.MaxBranches,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
            };
        }
    }
}
