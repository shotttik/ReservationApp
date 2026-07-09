using Application.Options;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class CacheService :ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly RedisOptions _redisOptions;
        private readonly TimeSpan _defaultExpiration;

        public CacheService(IConfiguration configuration, IDistributedCache cache, IOptions<RedisOptions> redisOptions)
        {
            _cache = cache;
            _redisOptions = redisOptions.Value;
            var expirationMinutes = configuration.GetValue<double>("Redis:CacheExpirationMinutes", 30);
            _defaultExpiration = TimeSpan.FromMinutes(expirationMinutes);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? _defaultExpiration
            };

            var json = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, json, options);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var json = await _cache.GetStringAsync(key);
            return json is null ? default : JsonSerializer.Deserialize<T>(json);
        }
        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
