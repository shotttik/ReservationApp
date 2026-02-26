using Application.Common.Requests.SubscriptionPlan;
using Application.Common.Results;
using Domain.Abstractions;
using Domain.DTO;

namespace Application.Interfaces
{
    public interface ICompanySubscriptionService
    {
        Task<Result<PagedList<CompanySubscriptionDTO>>> RetrievePaged(PagedParameters parameters);
        Task<Result> ChangePlan(int companyId, int subscriptionPlanId);
        Task<Result<CompanySubscriptionDTO>> Extend(int companyId, ExtendSubscriptionRequest request);
        Task<Result> Cancel(int companyId);
        Task<Result<CompanySubscriptionDTO>> Activate(int companyId, ActivateSubscriptionRequest request);
        Task<Result> SetAutoRenew(int companyId, SetAutoRenewRequest request);
        Task<Result<CompanySubscriptionDTO>> UpdatePeriod(int companyId, UpdateSubscriptionPeriodRequest request);
    }
}
