using Domain.Interfaces.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class, IBaseEntity
    {
        Task<T> Add(T entity);
        Task<T> Add(T entity, CancellationToken cancellationToken);
        Task<T?> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task Update(T entity);
        Task Delete(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task UpdateRange(IEnumerable<T> entities);
    }
}
