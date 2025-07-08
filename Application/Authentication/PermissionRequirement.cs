using Microsoft.AspNetCore.Authorization;

namespace Application.Authentication
{
    public class PermissionRequirement :IAuthorizationRequirement
    {
        public IReadOnlyList<string> Permissions { get; }

        public PermissionRequirement(string policy)
        {
            Permissions = policy.Split(',').Select(p => p.Trim()).ToList();
        }
    }
}
