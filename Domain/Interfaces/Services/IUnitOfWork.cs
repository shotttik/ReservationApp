namespace Domain.Interfaces.Services
{
    public interface IUnitOfWork :IDisposable
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
