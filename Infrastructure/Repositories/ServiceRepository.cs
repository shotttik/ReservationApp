using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class ServiceRepository :BaseRepository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
