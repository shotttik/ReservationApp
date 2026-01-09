using Application.Common.Results;
using Application.Interfaces;

namespace Application.Common.Security
{
    public class AccessGuard :IAccessGuard
    {
        private readonly IAuthService _authService;

        public AccessGuard(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Error> EnsureAccessToCompany(int routeCompanyId)
        {
            var user = await _authService.GetCurrentUser();

            if (user.IsSuperUser)
                return Error.None;

            if (user.CompanyID == null || user.CompanyID != routeCompanyId)
                return GenericResults.Forbidden;

            return Error.None;
        }

        public async Task<Error> EnsureAccessToCompanyEmployee(int employeeCompanyId, int employeeId)
        {
            var user = await _authService.GetCurrentUser();
            if (user.IsSuperUser)
                return Error.None;
            if (user.IsCompanyAdmin && user.CompanyID == employeeCompanyId)
                return Error.None;
            if (user.IsCompanyEmployee && user.CompanyID == employeeCompanyId && user.ID == employeeId)
                return Error.None;

            return GenericResults.Forbidden;
        }
        public async Task<Error> EnsureAccessToBooking(int bookingId, int? clientId, int employeeId, int companyId)
        {
            if (_authService.IsGuestForBooking(bookingId))
                return Error.None;

            var user = await _authService.GetCurrentUser();
            if (user.IsSuperUser)
                return Error.None;
            if (user.IsCompanyAdmin && user.CompanyID == companyId)
                return Error.None;
            if (user.IsCompanyEmployee && user.UserAccountId == employeeId)
                return Error.None;
            if (user.UserAccountId == clientId)
                return Error.None;

            return GenericResults.Forbidden;
        }

    }
}
