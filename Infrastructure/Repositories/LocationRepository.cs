using Domain.Entities.LocationReleated;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class LocationRepository :BaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
