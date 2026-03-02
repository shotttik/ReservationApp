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
                .Where(ua => ua.UserLoginData.ID == userLoginDataID)
                .Include(ua=> ua.UserLoginData)
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

        /// <summary>
        /// Get full user account data with UserLoginDataID.
        /// User, Branch and Company must all be active.
        /// </summary>
        /// <param name="userLoginDataID"></param>
        /// <returns>UserAccount or null</returns>

        public async Task<UserAccount?> GetEmployeeByUserLoginDataIDWithBookingData(int userLoginDataID)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var userAccount = await _dbSet
                .Where(e =>
                e.UserLoginData.ID == userLoginDataID && 
                e.UserLoginData.ActiveStatus == Domain.Enums.ActiveStatus.Active &&
                e.Branch != null &&
                e.Branch.ActiveStatus == Domain.Enums.ActiveStatus.Active &&
                e.Company != null &&
                e.Company.ActiveStatus == Domain.Enums.ActiveStatus.Active)
                .Include(e => e.Company)
                    .ThenInclude(c => c!.Services)
                .Include(e => e.Branch)
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
        /// <summary>
        /// Get full user account data with UserLoginDataID, user must be active.
        /// </summary>
        /// <param name="userLoginDataID"></param>
        /// <returns>UserAccount or null</returns>
        public async Task<UserAccount?> GetByUserLoginDataIDWithBookingData(int userLoginDataID)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var userAccount = await _dbSet
                .Where(e => e.UserLoginData.ID == userLoginDataID 
                && e.UserLoginData.ActiveStatus == Domain.Enums.ActiveStatus.Active)
                .Include(e => e.Company)
                    .ThenInclude(c => c!.Services)
                .Include(e => e.BookingsAsEmployee)
                .Include(e => e.WorkSchedules)
                .Include(e => e.WorkScheduleExceptions.Where(wse => wse.EndDate >= today))
                .FirstOrDefaultAsync();

            return userAccount;
        }
    }
}
