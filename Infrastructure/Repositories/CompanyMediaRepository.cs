using Domain.Entities.CompanyReleated;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompanyMediaRepository :ICompanyMediaRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<CompanyMedia> dbSet;

        public CompanyMediaRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<CompanyMedia>();
        }
        public async Task<CompanyMedia> Add(CompanyMedia entity, CancellationToken cancellationToken)
        {
            await dbSet.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
        public async Task<CompanyMedia> Update(CompanyMedia entity)
        {
            dbSet.Update(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
