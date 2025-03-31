using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserAccountRepository :BaseRepository<UserAccount>, IUserAccountRepository
    {
        private readonly ApplicationDbContext context;

        public UserAccountRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
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
    }
}
