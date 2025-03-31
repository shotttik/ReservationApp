using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserLoginDataRepository :BaseRepository<UserLoginData>, IUserLoginDataRepository
    {
        private readonly ApplicationDbContext context;

        public UserLoginDataRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<UserLoginData?> GetByEmail(string email)
        {
            return await _dbSet.Where(uld => uld.Email == email).FirstOrDefaultAsync();
        }
        public async Task UpdateRefreshToken(int ID, string? refreshToken, DateTime refreshTokenExpirationTime)
        {
            var userLoginData = await _dbSet.FindAsync(ID);
            userLoginData!.RefreshToken = refreshToken;
            userLoginData.RefreshTokenExpirationTime = refreshTokenExpirationTime;
            await context.SaveChangesAsync();
        }
        public async Task UpdateRecoveryToken(int ID, string? recoveryToken, DateTime recoveryTokenTime)
        {
            var userLoginData = await _dbSet.FindAsync(ID);
            userLoginData!.PasswordRecoveryToken = recoveryToken;
            userLoginData.RecoveryTokenTime = recoveryTokenTime;
            await context.SaveChangesAsync();
        }
        public async Task UpdateResetPasswordData(int ID, byte [] passwordHash, byte [] passwordSalt)
        {
            var userLoginData = await _dbSet.FindAsync(ID);
            userLoginData!.PasswordHash = passwordHash;
            userLoginData.PasswordSalt = passwordSalt;
            userLoginData.PasswordRecoveryToken = null;
            userLoginData.RecoveryTokenTime = null;
            await context.SaveChangesAsync();
        }

        public async Task<UserLoginData?> GetFullUserDataByEmail(string email)
        {
            var userLoginData = await _dbSet
                .Where(uld => uld.Email == email)
                .Include(u => u.UserAccount)
                .ThenInclude(ua => ua.Role)
                .ThenInclude(ur => ur!.Permissions)
                .FirstOrDefaultAsync();

            return userLoginData;
        }

        public async Task<UserLoginData?> GetFullUserData(int ID)
        {
            var userLoginData = await _dbSet
                .Where(uld => uld.ID == ID)
                .Include(u => u.UserAccount)
                .ThenInclude(ua => ua.Role)
                .ThenInclude(ur => ur!.Permissions)
                .FirstOrDefaultAsync();

            return userLoginData;
        }

        public async Task<UserLoginData?> GetByVerificationToken(string verificationToken)
        {
            return await _dbSet
                .Where(uld => uld.VerificationToken == verificationToken)
                .FirstOrDefaultAsync();
        }
    }
}
