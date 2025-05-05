using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.Swagger
{
    /// <summary>
    /// Custom ProblemDetails schema for Swagger documentation
    /// </summary>
    public class CustomProblemDetailsSchema :ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(ProblemDetails))
            {
                // Clear default properties
                schema.Properties.Clear();

                // Add standard ProblemDetails properties
                schema.Properties.Add("type", new OpenApiSchema { Type = "string", Example = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.1") });
                schema.Properties.Add("title", new OpenApiSchema { Type = "string", Example = new OpenApiString("Bad Request") });
                schema.Properties.Add("status", new OpenApiSchema { Type = "integer", Format = "int32", Example = new OpenApiInteger(400) });

                // Add custom errors array property
                var errorSchema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["code"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Login.InvalidPassword") },
                        ["description"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Invalid password") },
                        ["type"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Validation") }
                    }
                };

                var errorsArraySchema = new OpenApiSchema
                {
                    Type = "array",
                    Items = errorSchema
                };

                schema.Properties.Add("errors", errorsArraySchema);
            }
        }
    }
}