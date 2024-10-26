using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Middlewares
{
    public class ExtractEmailMiddleware
    {
        private readonly RequestDelegate next;

        public ExtractEmailMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers ["Authorization"].FirstOrDefault();
            if (authHeader is not null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken is not null)
                {
                    var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                    if (emailClaim is not null)
                    {
                        context.Items ["Email"] = emailClaim.Value;
                    }
                }
            }

            await next(context);
        }
    }
}
