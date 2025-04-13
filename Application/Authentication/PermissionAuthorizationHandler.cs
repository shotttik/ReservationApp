using Application.DTOs.User;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Shared.Utilities;
using System.Security.Claims;

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
            string? userID = context.User.Claims.FirstOrDefault(
                x => x.Type == ClaimTypes.Sid)?.Value;

            if (!int.TryParse(userID, out int parsedUserID))
            {
                return;
            }

            var user = await cacheService.GetAsync<UserAccountDTO>(CacheUtils.AuthorizationCacheKey(parsedUserID));

            if (user == null)
            {
                return;
            }
            var userPermissions = user.Role.Permissions;

            if (userPermissions.Any(p => p.Name == requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
