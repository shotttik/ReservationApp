using Domain.Entities.CompanyReleated;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ServiceRepository :BaseRepository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Service>> GetServicesByCompanyId(int companyId, bool forPublic)
        {
            var query = _dbSet.Where(s => s.CompanyID == companyId);

            if (forPublic)
                query = query.Where(s => s.ActiveStatus == Domain.Enums.ActiveStatus.Active);

            return await query.ToArrayAsync();
        }
        public async Task<Service?> Get(int id, int companyId)
        {
            return await _dbSet.Where(s => s.Id == id &&
            s.CompanyID == companyId &&
            s.ActiveStatus == Domain.Enums.ActiveStatus.Active).FirstOrDefaultAsync();
        }
    }
}
