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
            if (entity.IsMain)
            {
                await dbSet.Where(e => e.IsMain == true && e.CompanyID == entity.CompanyID)
                    .ExecuteUpdateAsync(Update => Update.SetProperty(e => e.IsMain, false), cancellationToken);
            }
            await dbSet.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
        public async Task<CompanyMedia> Update(CompanyMedia entity, CancellationToken cancellationToken)
        {
            dbSet.Update(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
        public async Task UpdateRange(IEnumerable<CompanyMedia> entities, CancellationToken cancellationToken)
        {
            dbSet.UpdateRange(entities);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task Delete(CompanyMedia entity, CancellationToken cancellationToken)
        {
            dbSet.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
