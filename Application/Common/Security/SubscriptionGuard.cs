using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO;
using Domain.Enums;
using Domain.Interfaces.Repositories;

namespace Application.Common.Security
{
    public class SubscriptionGuard :ISubscriptionGuard
    {
        private readonly ICompanySubscriptionRepository _companySubscriptionRepository;

        public SubscriptionGuard(ICompanySubscriptionRepository companySubscriptionRepository)
        {
            _companySubscriptionRepository = companySubscriptionRepository;
        }

        public async Task<Error> EnsureCanCreateEmployeeAsync(int companyId)
        {
            var (usage, error) = await GetValidatedUsage(companyId);
            if (usage == null || error != Error.None)
            {
                return error;
            }
            if (usage.EmployeeCount >= usage.MaxEmployees)
            {
                return CompanySubscriptionResults.EmployeeLimitReached;
            }
            return Error.None;
        }

        public async Task<Error> EnsureCanCreateBookingAsync(int companyId)
        {
            var (usage, error) = await GetValidatedUsage(companyId);
            if (usage == null || error != Error.None)
            {
                return error;
            }
            if (usage.MonthlyBookingCount >= usage.MaxBookingsPerMonth)
            {
                return CompanySubscriptionResults.BookingLimitReached;
            }

            return Error.None;
        }

        public async Task<(SubscriptionUsageDTO?, Error)> GetValidatedUsage(int companyId)
        {
            var usage = await _companySubscriptionRepository.GetSubscriptionData(companyId);

            if (usage == null)
            {
                return (null, CompanySubscriptionResults.NotFound);
            }

            if (usage.Status != SubscriptionStatus.Active)
            {
                return (null, CompanySubscriptionResults.IsNotActive);
            }
            if (usage.EndDate < DateTime.UtcNow)
            {
                return (null, CompanySubscriptionResults.Expired);
            }

            return (usage, Error.None);
        }
    }
}
