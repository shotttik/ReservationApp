using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class SubscriptionPlanService :ISubscriptionPlanService
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

        public SubscriptionPlanService(ISubscriptionPlanRepository subscriptionPlanRepository)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
        }

        public async Task<Result<IEnumerable<SubscriptionPlanDTO>>> GetAll()
        {
            var subscriptionPlans = await _subscriptionPlanRepository.GetAll();

            if (!subscriptionPlans.Any())
            {
                return Result.Failure<IEnumerable<SubscriptionPlanDTO>>(SubscriptionPlanResults.NotFound);
            }

            return Result.Success(subscriptionPlans.Select(e => e.MapToDTO()));
        }
    }
}
