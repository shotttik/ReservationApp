using Application.Common.Requests.SubscriptionPlan;
using Application.Common.Results;
using Domain.DTO;

namespace Application.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task<Result<SubscriptionPlanDTO>> Create(SubscriptionPlanCreateRequest request);
        Task<Result> Delete(int id);
        Task<Result<IEnumerable<SubscriptionPlanDTO>>> GetAll();
        Task<Result<SubscriptionPlanDTO>> Update(int id, SubscriptionPlanUpdateRequest request);
    }
}
