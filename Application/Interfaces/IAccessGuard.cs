using Application.Common.Results;

namespace Application.Interfaces
{
    public interface IAccessGuard
    {
        Task<Error> EnsureAccessToCompany(int routeCompanyId);
        Task<Error> EnsureAccessToCompanyEmployee(int employeeCompanyId, int employeeId);
        Task<Error> EnsureAccessToBooking(int bookingId,int? clientId, int employeeId, int companyId);

    }
}
