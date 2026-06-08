using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Application.Authentication
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute, IRequiredPermissionMetadata
    {
        public string PermissionName { get; }
        public IReadOnlyList<string> RequiredPermissions => new[] { PermissionName };

        public HasPermissionAttribute(Permission permission) :
            base(policy: permission.ToString())
        {
            PermissionName = permission.ToString();
        }
    }
}
