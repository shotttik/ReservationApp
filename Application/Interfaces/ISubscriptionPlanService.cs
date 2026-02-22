using Application.Common.Requests.SubscriptionPlan;
using Application.Common.Results;
using Domain.DTO;

namespace Application.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task<Result<IEnumerable<SubscriptionPlanDTO>>> GetAll();
        Task<Result<SubscriptionPlanDTO>> Update(int id, SubscriptionPlanUpdateRequest request);
    }
}
