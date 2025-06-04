using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserAccountRepository :BaseRepository<UserAccount>, IUserAccountRepository
    {

        public UserAccountRepository(
            ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public override async Task Update(UserAccount userAccount)
        {
            userAccount.UpdateTimestamp();
            _dbSet.Update(userAccount);
            await dbContext.SaveChangesAsync();
        }

        public async Task<UserAccount?> GetAuthorizationData(int ID)
        {
            var user = await _dbSet
                    .Include(u => u.Role)
                        .ThenInclude(r => r!.Permissions)
                    .Include(e => e.Company)
                        .ThenInclude(e => e!.WorkSchedules)
                    .FirstOrDefaultAsync(u => u.ID == ID);

            return user;
        }

        public override async Task<UserAccount?> Get(int ID)
        {
            var user = await _dbSet
                .Include(u => u.Role)
                    .ThenInclude(r => r!.Permissions)
                .Include(c => c.Company)
                .FirstOrDefaultAsync(u => u.ID == ID);

            return user;
        }

        public async Task<UserAccount?> GetByUserLoginDataID(int userLoginDataID)
        {
            var userAccount = await _dbSet
                .Where(e => e.UserLoginData != null && e.UserLoginData.ID == userLoginDataID)
                .FirstOrDefaultAsync();

            return userAccount;
        }
    }
}
