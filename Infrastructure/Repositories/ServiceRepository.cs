using Domain.Entities.CompanyReleated;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class ServiceRepository :BaseRepository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
