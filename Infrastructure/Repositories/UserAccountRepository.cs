using Application.Extensions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Utilities;

namespace Infrastructure.Repositories
{
    public class UserAccountRepository :BaseRepository<UserAccount>, IUserAccountRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ICacheService cache;

        public UserAccountRepository(
            ApplicationDbContext context,
            ICacheService cache
            ) : base(context)
        {
            this.context = context;
            this.cache = cache;
        }
        public override async Task Update(UserAccount userAccount)
        {
            _dbSet.Update(userAccount);
            await cache.SetAsync(CacheUtils.AuthorizationCacheKey(userAccount.ID), userAccount.MapToAuthorizationData());
            await context.SaveChangesAsync();
        }

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
