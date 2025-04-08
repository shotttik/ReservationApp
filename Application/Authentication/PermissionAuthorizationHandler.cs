using Application.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using System.Text.Json;

namespace Application.Authentication
{
    public class PermissionAuthorizationHandler
        :AuthorizationHandler<PermissionRequirement>
    {
        private readonly IDistributedCache cache;

        public PermissionAuthorizationHandler(
            IDistributedCache cache)
        {
            this.cache = cache;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            string? userID = context.User.Claims.FirstOrDefault(
                x => x.Type == ClaimTypes.Sid)?.Value;

            if (!int.TryParse(userID, out int parsedUserID))
            {
                return;
            }

            var cachedData = await cache.GetStringAsync(GetCacheKey(parsedUserID));

            if (string.IsNullOrEmpty(cachedData))
            {
                return;
            }
            var user = JsonSerializer.Deserialize<UserAccountDTO>(cachedData)!;
            var userPermissions = user.Role.Permissions;

            if (userPermissions.Any(p => p.Name == requirement.Permission))
            {
                context.Succeed(requirement);
            }

        }
        private string GetCacheKey(int userID) => $"UserAuthorization:{userID}";

    }
}
