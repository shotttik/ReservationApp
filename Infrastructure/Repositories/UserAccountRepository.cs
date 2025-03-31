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
            var user = await _dbSet
                    .Include(u => u.Role)
                        .ThenInclude(r => r!.Permissions)
                    .FirstOrDefaultAsync(u => u.ID == ID);

            return user;
        }

        public override async Task<UserAccount?> Get(int ID)
        {
            var user = await _dbSet
                .Include(u => u.Role)
                    .ThenInclude(r => r!.Permissions)
                .FirstOrDefaultAsync(u => u.ID == ID);

            return user;
        }
    }
}
