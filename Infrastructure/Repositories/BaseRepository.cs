using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T> :IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext dbContext;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T?> Get(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
