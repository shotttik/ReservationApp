using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UserAccountRepository :IUserAccountRepository
    {
        private readonly UserDbContext context;

        public UserAccountRepository(UserDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(UserAccount userAccount)
        {
            await context.UserAccounts.AddAsync(userAccount);
            await context.SaveChangesAsync();

            return userAccount.ID;
        }
    }
}
