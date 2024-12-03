using Microsoft.AspNetCore.Authorization;

namespace Application.Authentication
{
    public class PermissionRequirement: IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
        public string Permission { get; }

    }
}
