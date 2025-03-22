using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserAccountRepository :IUserAccountRepository
    {
        private readonly ApplicationDbContext context;

        public UserAccountRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(UserAccount userAccount)
        {
            await context.UserAccounts.AddAsync(userAccount);
            await context.SaveChangesAsync();

            return userAccount.ID;
        }

        public async Task<UserAccount?> GetAuthorizationData(int ID)
        {
            var user = await context.UserAccounts
                    .Include(u => u.Role)
                        .ThenInclude(r => r!.Permissions)
                    .FirstOrDefaultAsync(u => u.ID == ID);

            return user;
        }

        public async Task<UserAccount?> GetUserAccountByID(int ID)
        {
            var user = await context.UserAccounts
                .Include(u => u.Role)
                    .ThenInclude(r => r!.Permissions)
                .FirstOrDefaultAsync(u => u.ID == ID);

            return user;
        }
        public async Task UpdateUserAccount(UserAccount userAccount)
        {
            context.UserAccounts.Update(userAccount);
            await context.SaveChangesAsync();
        }
    }
}
