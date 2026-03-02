using Application.Common.Requests.SubscriptionPlan;
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
                Id = entity.Id,
                Name = entity.Name,
                PriceMonthly = entity.PriceMonthly,
                MaxEmployees = entity.MaxEmployees,
                MaxBookingsPerMonth = entity.MaxBookingsPerMonth,
                MaxBranches = entity.MaxBranches,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
            };
        }

        public static void ApplyTo(this SubscriptionPlanUpdateRequest request, SubscriptionPlan existingEntity)
        {
            existingEntity.Name = request.Name;
            existingEntity.PriceMonthly = request.PriceMonthly;
            existingEntity.MaxEmployees = request.MaxEmployees;
            existingEntity.MaxBookingsPerMonth = request.MaxBookingsPerMonth;
            existingEntity.MaxBranches = request.MaxBranches;
        }
    }
}
