using Application.Common.Results;

namespace Application.Interfaces
{
    public interface ISubscriptionGuard
    {
        Task<Error> EnsureCanCreateEmployeeAsync(int companyId);
        Task<Error> EnsureCanCreateBookingAsync(int companyId);
    }
}
