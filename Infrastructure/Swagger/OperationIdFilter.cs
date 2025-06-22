using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.Swagger
{
    public class OperationIdFilter :IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (string.IsNullOrEmpty(operation.OperationId))
            {
                var controller = context.ApiDescription.ActionDescriptor.RouteValues ["controller"];
                var action = context.ApiDescription.ActionDescriptor.RouteValues ["action"];
                var httpMethod = context.ApiDescription.HttpMethod;

                // Extract version from route or group name
                var version = context.ApiDescription.GroupName ?? "v1";

                operation.OperationId = $"{version}_{controller}_{action}_{httpMethod}";
            }
        }
    }

}
