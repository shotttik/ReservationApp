using Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Services
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction!.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction!.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }
}
