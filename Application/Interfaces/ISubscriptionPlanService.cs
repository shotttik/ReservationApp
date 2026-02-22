using Application.Common.Results;
using Domain.DTO;

namespace Application.Interfaces
{
    public interface ISubscriptionPlanService
    {
        Task<Result<IEnumerable<SubscriptionPlanDTO>>> GetAll();
    }
}
