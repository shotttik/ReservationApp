namespace Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T?> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task Update(T entity);
        Task Delete(T entity);
    }
}
