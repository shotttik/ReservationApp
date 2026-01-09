using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Application.Authentication
{
    public sealed class GuestOrPermissionAttribute :AuthorizeAttribute
    {
        public GuestOrPermissionAttribute(params Permission [] permissions)
        {
            Policy = $"{GuestOrPermissionPolicyPrefix}{string.Join(",", permissions.Select(p => p.ToString()))}";
        }

        public const string GuestOrPermissionPolicyPrefix = "GuestOrPermission:";
    }

}
