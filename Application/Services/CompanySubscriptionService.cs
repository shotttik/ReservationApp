using Application.Common.Requests.SubscriptionPlan;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Extensions.Mappers.Pagination;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class CompanySubscriptionService :ICompanySubscriptionService
    {
        private readonly ICompanySubscriptionRepository _companySubscriptionRepository;
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

        public CompanySubscriptionService(
            ICompanySubscriptionRepository companySubscriptionRepository,
            ISubscriptionPlanRepository subscriptionPlanRepository)
        {
            _companySubscriptionRepository = companySubscriptionRepository;
            _subscriptionPlanRepository = subscriptionPlanRepository;
        }

        public async Task<Result<PagedList<CompanySubscriptionDTO>>> RetrievePaged(PagedParameters parameters)
        {
            var allowedFields = CompanySubscriptionFieldMap.DtoToEntityPath;
            var errors = parameters.Validate(allowedFields, typeof(CompanySubscriptionDTO));
            if (errors.Any())
            {
                return Result.Failure<PagedList<CompanySubscriptionDTO>>(PagedListResults.InvalidPagedParameters(errors.First()));
            }

            var companySubscriptions = await _companySubscriptionRepository.RetrievePaged(parameters);

            return companySubscriptions;
        }


        public async Task<Result<CompanySubscriptionDTO>> Activate(int companyId, ActivateSubscriptionRequest request)
        {
            var companySubscription = await _companySubscriptionRepository.GetByCompanyId(companyId);
            if (companySubscription == null)
            {
                return Result.Failure<CompanySubscriptionDTO>(CompanySubscriptionResults.NotFound);
            }
            companySubscription.Activate(request.StartDate, request.EndDate);

            await _companySubscriptionRepository.Update(companySubscription);

            return companySubscription.MapToDTO();
        }

        // can be accessed by company admin and super admin
        public async Task<Result> Cancel(int companyId)
        {
            var companySubscription = await _companySubscriptionRepository.GetByCompanyId(companyId);
            if (companySubscription == null)
            {
                return Result.Failure(CompanySubscriptionResults.NotFound);
            }
            companySubscription.Cancel();
            await _companySubscriptionRepository.Update(companySubscription);

            return Result.Success(CompanySubscriptionResults.Canceled);
        }

        public async Task<Result> ChangePlan(int companyId, int subscriptionPlanId)
        {
            var companySubscription = await _companySubscriptionRepository.GetByCompanyId(companyId);
            if (companySubscription == null)
            {
                return Result.Failure(CompanySubscriptionResults.NotFound);
            }

            var subscriptionPlan = await _subscriptionPlanRepository.Get(subscriptionPlanId);
            if (subscriptionPlan == null)
            {
                return Result.Failure(SubscriptionPlanResults.NotFound);
            }
            companySubscription.SubscriptionPlan = subscriptionPlan;
            companySubscription.SubscriptionPlanId = subscriptionPlanId;

            await _companySubscriptionRepository.Update(companySubscription);

            return Result.Success(CompanySubscriptionResults.PlanChanged);
        }

        public async Task<Result<CompanySubscriptionDTO>> Extend(int companyId, ExtendSubscriptionRequest request)
        {
            var companySubscription = await _companySubscriptionRepository.GetByCompanyId(companyId);
            if (companySubscription == null)
            {
                return Result.Failure<CompanySubscriptionDTO>(CompanySubscriptionResults.NotFound);
            }
            if (!companySubscription.IsExtendable)
            {
                return Result.Failure<CompanySubscriptionDTO>(CompanySubscriptionResults.IsnotExtendable);
            }
            companySubscription.Extend(request.AdditionalMonths);
            await _companySubscriptionRepository.Update(companySubscription);

            return Result.Success<CompanySubscriptionDTO>(companySubscription.MapToDTO(), CompanySubscriptionResults.PlanExtended);
        }
        public async Task<Result> SetAutoRenew(int companyId, SetAutoRenewRequest request)
        {
            var companySubscription = await _companySubscriptionRepository.GetByCompanyId(companyId);
            if (companySubscription == null)
            {
                return Result.Failure(CompanySubscriptionResults.NotFound);
            }

            if (!companySubscription.IsActive)
            {
                return Result.Failure(CompanySubscriptionResults.IsnotAutoRenewable);

            }
            if (companySubscription.AutoRenew == request.AutoRenew)
            {
                return Result.Success(CompanySubscriptionResults.AutoRenewSet);
            }
            companySubscription.AutoRenew = request.AutoRenew;
            await _companySubscriptionRepository.Update(companySubscription);

            return Result.Success(CompanySubscriptionResults.AutoRenewSet);
        }

        public async Task<Result<CompanySubscriptionDTO>> UpdatePeriod(int companyId, UpdateSubscriptionPeriodRequest request)
        {
            var companySubscription = await _companySubscriptionRepository.GetByCompanyId(companyId);
            if (companySubscription == null)
            {
                return Result.Failure<CompanySubscriptionDTO>(CompanySubscriptionResults.NotFound);
            }

            if (!companySubscription.IsActive)
            {
                return Result.Failure<CompanySubscriptionDTO>(CompanySubscriptionResults.IsNotActive);
            }
            companySubscription.UpdatePeriod(request.StartDate, request.EndDate);
            await _companySubscriptionRepository.Update(companySubscription);

            return Result.Success<CompanySubscriptionDTO>(companySubscription.MapToDTO(), CompanySubscriptionResults.PeriodUpdated);
        }
    }
}
