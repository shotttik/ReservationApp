using Domain.DTO;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Shared.Utilities;

namespace Application.Authentication
{
    public sealed class GuestOrUserAuthorizationHandler
        :AuthorizationHandler<GuestOrUserRequirement>
    {
        private readonly ICacheService _cache;

        public GuestOrUserAuthorizationHandler(ICacheService cache)
        {
            _cache = cache;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GuestOrUserRequirement requirement)
        {
            // A) Guest access
            if (IsGuestForThisBooking(context))
            {
                context.Succeed(requirement);
                return;
            }

            // B) Staff permission access (same model you already have)
            if (await IsAuthorized(context))
            {
                context.Succeed(requirement);
            }

        }
        private static bool IsGuestForThisBooking(AuthorizationHandlerContext context)
        {
            var scope = context.User.FindFirst("scope")?.Value;
            var bookingIdClaim = context.User.FindFirst("bookingId")?.Value;

            if (scope != "booking:guest") return false;
            if (!int.TryParse(bookingIdClaim, out var bookingIdFromToken)) return false;

            if (context.Resource is HttpContext httpContext)
            {
                var routeValues = httpContext.Request.RouteValues;

                if (routeValues.TryGetValue("id", out var idObj) &&
                    int.TryParse(idObj?.ToString(), out var routeId))
                {
                    return routeId == bookingIdFromToken;
                }
            }

            return false;
        }

        private async Task<bool> IsAuthorized(
            AuthorizationHandlerContext context)
        {
            var sessionID = context.User.Claims.FirstOrDefault(x => x.Type == "SessionID")?.Value;
            if (string.IsNullOrWhiteSpace(sessionID)) return false;

            var sessionInfo = await _cache.GetAsync<SessionInfoDTO>(CacheUtils.SessionKey(sessionID));
            if (sessionInfo == null) return false;

            return true;
        }
    }
}
