using Domain.Entities.User;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserAccountMediaRepository :IUserAccountMediaRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<UserAccountMedia> _dbSet;

        public UserAccountMediaRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            _dbSet = dbContext.Set<UserAccountMedia>();
        }

        public async Task EmptyThenAdd(UserAccountMedia entity, CancellationToken cancellationToken)
        {
            await _dbSet.Where(e => e.UserAccountId == entity.UserAccountId)
                .ExecuteDeleteAsync(cancellationToken);
            await _dbSet.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
