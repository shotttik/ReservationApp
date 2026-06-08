using Application.Authentication;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Configuration.Swagger
{
    public class RequiredPermissionsOperationFilter : IOperationFilter
    {
        private const string RequiredPermissionsExtensionName = "x-required-permissions";

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var metadataPermissions = context.ApiDescription.ActionDescriptor.EndpointMetadata
                .OfType<IRequiredPermissionMetadata>()
                .SelectMany(metadata => metadata.RequiredPermissions);

            var attributePermissions = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<IRequiredPermissionMetadata>()
                .SelectMany(metadata => metadata.RequiredPermissions)
                ?? Enumerable.Empty<string>();

            var requiredPermissions = metadataPermissions
                .Concat(attributePermissions)
                .Where(permission => !string.IsNullOrWhiteSpace(permission))
                .Distinct(StringComparer.Ordinal)
                .OrderBy(permission => permission, StringComparer.Ordinal)
                .ToArray();

            if (requiredPermissions.Length == 0)
            {
                return;
            }

            var extension = new OpenApiArray();
            foreach (var permission in requiredPermissions)
            {
                extension.Add(new OpenApiString(permission));
            }

            operation.Extensions[RequiredPermissionsExtensionName] = extension;
        }
    }
}
