using Microsoft.AspNetCore.Authorization;

namespace Application.Authentication
{
    public sealed class GuestOrPermissionRequirement :IAuthorizationRequirement
    {
        public IReadOnlyList<string> Permissions { get; }

        public GuestOrPermissionRequirement(string csv)
        {
            Permissions = csv.Split(',', StringSplitOptions.RemoveEmptyEntries)
                             .Select(p => p.Trim())
                             .ToList();
        }
    }
}
