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
