using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Application.Authentication
{
    public class PermissionAuthorizationPolicyProvider
        :DefaultAuthorizationPolicyProvider
    {
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }
        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(GuestOrPermissionAttribute.GuestOrPermissionPolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var csv = policyName.Substring(GuestOrPermissionAttribute.GuestOrPermissionPolicyPrefix.Length);

                return new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, "Guest")
                    .RequireAuthenticatedUser()
                    .AddRequirements(new GuestOrPermissionRequirement(csv))
                    .Build();
            }

            AuthorizationPolicy? policy = await base.GetPolicyAsync(policyName);
            if (policy == null)
            {
                policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(policyName))
                    .Build();
            }
            return policy;
        }
    }

}
