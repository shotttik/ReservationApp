using Domain.DTO;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Shared.Utilities;

namespace Application.Authentication
{
    public class PermissionAuthorizationHandler
        :AuthorizationHandler<PermissionRequirement>
    {
        private readonly ICacheService cacheService;

        public PermissionAuthorizationHandler(
            ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            string? sessionID = context.User.Claims.FirstOrDefault(
                x => x.Type == "SessionID")?.Value;

            if (sessionID == null)
            {
                return;
            }

            var sessionInfo = await cacheService.GetAsync<SessionInfoDTO>(CacheUtils.SessionKey(sessionID));

            if (sessionInfo == null)
            {
                return;
            }
            var userPermissions = sessionInfo.AuthUser.Role.Permissions;

            if (userPermissions.Any(p => p.Name == requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
