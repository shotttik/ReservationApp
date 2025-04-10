using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Utilities;
using System.Security.Claims;
using System.Text.Json;

namespace Application.Services
{
    public class AuthService :IAuthService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly IDistributedCache cache;

        public AuthService(IHttpContextAccessor httpContextAccessor,
            IUserLoginDataRepository userLoginDataRepository,
            IDistributedCache cache)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userLoginDataRepository = userLoginDataRepository;
            this.cache = cache;
        }

        public async Task<UserAccountDTO> GetCurrentUser()
        {
            var userIDClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Sid)?.Value;

            if (!int.TryParse(userIDClaim, out var userID))
                throw new AuthorizationException("Invalid or missing user ID in token.");

            var cachedData = await cache.GetStringAsync(RedisUtils.AuthorizationCacheKey(userID));

            if (string.IsNullOrEmpty(cachedData))
            {
                throw new AuthorizationException("Authenticated user not found.");

            }

            return JsonSerializer.Deserialize<UserAccountDTO>(cachedData)!;
        }
    }
}
