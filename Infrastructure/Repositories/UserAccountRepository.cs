using Application.Extensions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class UserAccountRepository :BaseRepository<UserAccount>, IUserAccountRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IDistributedCache cache;
        private TimeSpan _cacheExpiration;

        public UserAccountRepository(
            ApplicationDbContext context,
            IDistributedCache cache,
            IConfiguration configuration
            ) : base(context)
        {
            this.context = context;
            this.cache = cache;
            _cacheExpiration = TimeSpan.FromMinutes(Convert.ToDouble(configuration ["Redis:CacheExpirationMinutes"]));

        }
        public override async Task Update(UserAccount userAccount)
        {
            _dbSet.Update(userAccount);
            var serializedData = JsonSerializer.Serialize(userAccount.MapToAuthorizationData());
            await cache.SetStringAsync(GetCacheKey(userAccount.ID), serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheExpiration
            });
            await context.SaveChangesAsync();
        }
        private string GetCacheKey(int userID) => $"UserAuthorization:{userID}";

        public async Task<UserAccount?> GetAuthorizationData(int ID)
        {
            var user = await _dbSet
                    .Include(u => u.Role)
                        .ThenInclude(r => r!.Permissions)
                    .FirstOrDefaultAsync(u => u.ID == ID);

            return user;
        }

        public override async Task<UserAccount?> Get(int ID)
        {
            var user = await _dbSet
                .Include(u => u.Role)
                    .ThenInclude(r => r!.Permissions)
                .Include(c => c.Company)
                .FirstOrDefaultAsync(u => u.ID == ID);

            return user;
        }
    }
}
