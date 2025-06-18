using Domain.Entities.Common;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class MediaRepository :BaseRepository<Media>, IMediaRepository
    {
        public MediaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
