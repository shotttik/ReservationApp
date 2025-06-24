using Application.Common.Results;
using Application.Interfaces;

namespace Application.Common.Security
{
    public class CompanyAccessGuard
    {
        private readonly IAuthService _authService;

        public CompanyAccessGuard(IAuthService authService)
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
    }
}
