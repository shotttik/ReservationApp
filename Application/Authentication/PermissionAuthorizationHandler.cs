using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Authentication
{
    public class PermissionAuthorizationHandler
        :AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserService userService;

        public PermissionAuthorizationHandler(IUserService userService)
        {
            this.userService = userService;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            string? userID = context.User.Claims.FirstOrDefault(
                x => x.Type == ClaimTypes.PrimarySid)?.Value;

            if (!int.TryParse(userID, out int parsedUserID))
            {
                return;
            }

            var user = await userService.GetUserAuthorizationDataAsync();
            if (!user.IsSuccess)
            {
                return;
            }

            var userPermissions = user.Value.Role.Permissions;
            if (userPermissions.Any(p => p.Name == requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
