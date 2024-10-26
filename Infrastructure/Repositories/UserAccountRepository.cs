using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<UserAccount?> GetAuthorizationData(int ID)
        {
            var user = await context.UserAccounts
                    .Include(u => u.Role)
                        .ThenInclude(r => r.Permissions)
                    .FirstOrDefaultAsync(u => u.ID == ID);

            return user;
        }
    }
}
