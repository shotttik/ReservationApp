using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Domain.Repositories
{
    public class UserAccountRepository: IUserAccountRepository
    {
        private readonly UserAccountDbContext context;

        public UserAccountRepository(UserAccountDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(UserAccount userAccount)
        {
            await context.UserAccounts.AddAsync(userAccount);
            await context.SaveChangesAsync();
        }
    }
}
