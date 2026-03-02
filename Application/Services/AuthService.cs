using Application.Exceptions;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO;
using Domain.DTO.User;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Utilities;
using System.Security.Claims;

namespace Application.Services
{
    public class AuthService :IAuthService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICacheService cacheService;
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly IConfiguration configuration;

        public AuthService(IHttpContextAccessor httpContextAccessor,
            ICacheService cacheService,
            IUserLoginDataRepository userLoginDataRepository, IConfiguration configuration
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.cacheService = cacheService;
            this.userLoginDataRepository = userLoginDataRepository;
            this.configuration = configuration;
        }

        public async Task<AuthUser> GetCurrentUser()
        {
            var sessionID = httpContextAccessor.HttpContext?.User?.FindFirst("SessionID")?.Value;

            if (sessionID.IsNullOrEmpty())
                throw new AuthorizationException("Invalid or missing user ID in token.");

            var sessionInfo = await cacheService.GetAsync<SessionInfoDTO>(CacheUtils.SessionKey(sessionID!));
            return sessionInfo == null ? throw new AuthorizationException("Authenticated user not found.") : sessionInfo.AuthUser;
        }

        public string GetEmail() => GetClaim(ClaimTypes.Email);
        public string? GetRoleOrNull() => GetClaimOrNull(ClaimTypes.Role);
        public string GetSessionID() => GetClaim("SessionID");
        public int GetUserAccountID() => GetIntClaim(ClaimTypes.Sid);
        public int GetUserLoginDataID() => GetIntClaim(ClaimTypes.PrimarySid);
        public string? GetAccessToken()
        {
            var accessToken = httpContextAccessor.HttpContext?.Request.Headers ["Authorization"].ToString()?.Replace("Bearer ", "");
            return accessToken;
        }

        // refresh AuthUser cache
        public async Task RefreshUserCache(int? userLoginDataID = null)
        {
            var userId = userLoginDataID ?? GetUserLoginDataID();
            var user = await userLoginDataRepository.GetFullUserData(userId);
            if (user == null) return;

            var authUser = user.MapToAuthorizationData();
            var sessionIds = await cacheService.GetAsync<List<string>>(CacheUtils.ActiveSessionsKey(userId));

            if (sessionIds == null) return;

            foreach (var sessionId in sessionIds)
            {
                var sessionKey = CacheUtils.SessionKey(sessionId);
                var session = await cacheService.GetAsync<SessionInfoDTO>(sessionKey);
                if (session != null)
                {
                    session.AuthUser = authUser;
                    await ResetSessionAsync(sessionKey, session);
                }
            }
        }
        public Task RefreshAuthUserCache() => RefreshUserCache();
        public bool IsGuestForBooking(int bookingId)
        {
            var user = httpContextAccessor.HttpContext?.User;

            var scope = user?.FindFirst("scope")?.Value;
            if (scope != "booking:guest") return false;

            var bookingIdClaim = user?.FindFirst("bookingId")?.Value;
            if (!int.TryParse(bookingIdClaim, out var tokenBookingId)) return false;

            return tokenBookingId == bookingId;
        }
        private async Task ResetSessionAsync(string sessionKey, SessionInfoDTO session)
        {
            // Reset TTL (optional — can be kept same or re-applied)
            var ttl = session.RefreshTokenExpTime - DateTime.UtcNow;
            if (ttl <= TimeSpan.Zero)
            {
                var expDays = Convert.ToDouble(configuration ["Jwt:RefreshTokenExpirationDays"]);
                ttl = TimeSpan.FromDays(expDays);
            }
            await cacheService.SetAsync(sessionKey, session, ttl);
        }
        private string GetClaim(string type)
        {
            var value = httpContextAccessor.HttpContext?.User?.FindFirst(type)?.Value;
            if (string.IsNullOrEmpty(value))
                throw new AuthorizationException($"{type} claim is not available.");
            return value;
        }
        private string? GetClaimOrNull(string type)
        {
            var value = httpContextAccessor.HttpContext?.User?.FindFirst(type)?.Value;
            return value;
        }
        private int GetIntClaim(string type)
        {
            var claimValue = GetClaim(type);
            if (!int.TryParse(claimValue, out var result))
                throw new AuthorizationException($"{type} claim is not a valid integer.");
            return result;
        }
    }
}
