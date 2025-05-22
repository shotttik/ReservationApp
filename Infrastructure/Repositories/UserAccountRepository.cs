using Application.Extensions.Mappers;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Utilities;

namespace Infrastructure.Repositories
{
    public class UserAccountRepository :BaseRepository<UserAccount>, IUserAccountRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICacheService cache;

        public UserAccountRepository(
            ApplicationDbContext dbContext,
            ICacheService cache) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.cache = cache;
        }
        public override async Task Update(UserAccount userAccount)
        {
            userAccount.UpdateTimestamp();
            _dbSet.Update(userAccount);
            await cache.SetAsync(CacheUtils.AuthorizationCacheKey(userAccount.ID), userAccount.MapToAuthorizationData());
            await dbContext.SaveChangesAsync();
        }

        public async Task<UserAccount?> GetAuthorizationData(int ID)
        {
            var user = await _dbSet
                    .Include(u => u.Role)
                        .ThenInclude(r => r!.Permissions)
                    .Include(e => e.Company)
                        .ThenInclude(e => e!.WorkSchedules)
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

        public async Task<PagedList<UserAccountDTO>> RetrievePaged(
            PagedParameters parameters,
            CancellationToken cancellationToken,
            int authUserID)
        {
            var query = _dbSet.AsQueryable();

            query = query.ApplyQueryParamsAsync(parameters);

            var totalCount = await query.CountAsync();

            var users = await query
                .Include(u => u.Role)
                .Include(c => c.Company)
                .Where(u => u.ID != authUserID)
                .Select(e => e.MapToAuthorizationData())
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<UserAccountDTO>(users, parameters.PageNumber, parameters.PageSize, totalCount);
        }
    }
}
