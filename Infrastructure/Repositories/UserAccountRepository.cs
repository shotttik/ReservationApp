using Domain.Entities.User;
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
                .Where(e => e.UserLoginData.ID == userLoginDataID)
                .FirstOrDefaultAsync();

            return userAccount;
        }

        public async Task<UserAccount?> GetByUserLoginDataIDWithWorkSchedules(int userLoginDataID)
        {
            var userAccount = await _dbSet
                .Where(e => e.UserLoginData.ID == userLoginDataID)
                .Include(e => e.WorkSchedules)
                .FirstOrDefaultAsync();

            return userAccount;
        }
        public async Task<UserAccount?> GetByUserLoginDataIDWithWorkScheduleExceptions(int userLoginDataID)
        {
            var userAccount = await _dbSet
                .Where(e => e.UserLoginData.ID == userLoginDataID)
                .Include(e => e.WorkScheduleExceptions)
                .FirstOrDefaultAsync();

            return userAccount;
        }

        public async Task<UserAccount?> GetByUserLoginDataIDWithBookingData(int userLoginDataID)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var userAccount = await _dbSet
                .Where(e => e.UserLoginData.ID == userLoginDataID)
                .Include(e => e.Company)
                    .ThenInclude(c => c!.Services)
                .Include(e => e.BookingsAsEmployee)
                .Include(e => e.WorkSchedules)
                .Include(e => e.WorkScheduleExceptions.Where(wse => wse.EndDate >= today))
                .FirstOrDefaultAsync();

            return userAccount;
        }

        public async Task<UserAccount?> GetByEmailWithClientBookingData(string email)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var userAccount = await _dbSet
                .Where(e => e.UserLoginData.Email == email)
                .Include(e => e.BookingsAsClient)
                .Include(e => e.UserLoginData)
                .FirstOrDefaultAsync();

            return userAccount;
        }
    }
}
