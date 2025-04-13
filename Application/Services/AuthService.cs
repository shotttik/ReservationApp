using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Utilities;
using System.Security.Claims;

namespace Application.Services
{
    public class AuthService :IAuthService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICacheService cacheService;

        public AuthService(IHttpContextAccessor httpContextAccessor,
            ICacheService cacheService
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.cacheService = cacheService;
        }

        public async Task<UserAccountDTO> GetCurrentUser()
        {
            var userIDClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Sid)?.Value;

            if (!int.TryParse(userIDClaim, out var userID))
                throw new AuthorizationException("Invalid or missing user ID in token.");

            var userAccountDTO = await cacheService.GetAsync<UserAccountDTO>(CacheUtils.AuthorizationCacheKey(userID));

            return userAccountDTO ?? throw new AuthorizationException("Authenticated user not found.");
        }
    }
}
