using Application.Common.Results;

namespace Application.Interfaces
{
    public interface ISubscriptionGuard
    {
        Task<Error> EnsurCanCreateEmployeeAsync(int companyId);
        Task<Error> EnsureCanCreateBookingAsync(int companyId);
    }
}
