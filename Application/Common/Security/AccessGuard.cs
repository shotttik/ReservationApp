using Application.Common.Results;
using Application.Interfaces;

namespace Application.Common.Security
{
    public class AccessGuard:IAccessGuard
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

        public async Task<Error> EnsureAccessToCompanyMember(int memberCompanyId, int memberId)
        {
            var user = await _authService.GetCurrentUser();
            if (user.IsSuperUser)
                return Error.None;
            if (user.IsCompanyAdmin && user.CompanyID == memberCompanyId)
                return Error.None;
            if (user.IsCompanyMember && user.CompanyID == memberCompanyId && user.ID == memberId)
                return Error.None;
            return GenericResults.Forbidden;
        }
    }
}
