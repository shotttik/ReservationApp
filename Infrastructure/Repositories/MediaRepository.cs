using Domain.Entities.Common;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MediaRepository :BaseRepository<Media>, IMediaRepository
    {
        public MediaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> Exists(IEnumerable<int> ids)
        {
            var allExist = await _dbSet
                    .Where(e => ids.Contains(e.ID))
                    .CountAsync() == ids.Count();

            return allExist;
        }

        public async Task<HashSet<string>> GetAllRemoteUrls()
        {
            var list = await _dbSet
                .Select(e => e.RemoteUrl)
                .ToListAsync();

            return [.. list];
        }
    }
}
