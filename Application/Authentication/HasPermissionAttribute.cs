using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Application.Authentication
{
    public sealed class HasPermissionAttribute :AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission) :
            base(policy: permission.ToString())
        {
        }
    }
}
