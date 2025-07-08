using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Application.Authentication
{
    public sealed class HasAnyPermissionAttribute :AuthorizeAttribute
    {
        public HasAnyPermissionAttribute(params Permission [] permissions)
        {
            Policy = string.Join(",", permissions.Select(p => p.ToString()));
        }
    }
}
