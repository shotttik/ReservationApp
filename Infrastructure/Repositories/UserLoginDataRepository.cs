using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserLoginDataRepository :IUserLoginDataRepository
    {
        private readonly ApplicationDbContext context;

        public UserLoginDataRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(UserLoginData userLoginData)
        {
            await context.UserLoginDatas.AddAsync(userLoginData);
            await context.SaveChangesAsync();
        }

        public async Task Update(UserLoginData userLoginData)
        {
            context.UserLoginDatas.Update(userLoginData);
            await context.SaveChangesAsync();
        }
        public async Task<UserLoginData?> GetByEmailAsync(string email)
        {
            return await context.UserLoginDatas.Where(uld => uld.Email == email).FirstOrDefaultAsync();
        }
        public async Task UpdateRefreshToken(int ID, string? refreshToken, DateTime refreshTokenExpirationTime)
        {
            var userLoginData = await context.UserLoginDatas.FindAsync(ID);
            userLoginData!.RefreshToken = refreshToken;
            userLoginData.RefreshTokenExpirationTime = refreshTokenExpirationTime;
            await context.SaveChangesAsync();
        }
        public async Task UpdateRecoveryToken(int ID, string? recoveryToken, DateTime recoveryTokenTime)
        {
            var userLoginData = await context.UserLoginDatas.FindAsync(ID);
            userLoginData!.PasswordRecoveryToken = recoveryToken;
            userLoginData.RecoveryTokenTime = recoveryTokenTime;
            await context.SaveChangesAsync();
        }
        public async Task UpdateResetPasswordData(int ID, byte [] passwordHash, byte [] passwordSalt)
        {
            var userLoginData = await context.UserLoginDatas.FindAsync(ID);
            userLoginData!.PasswordHash = passwordHash;
            userLoginData.PasswordSalt = passwordSalt;
            userLoginData.PasswordRecoveryToken = null;
            userLoginData.RecoveryTokenTime = null;
            await context.SaveChangesAsync();
        }

        public async Task<UserLoginData?> GetFullUserDataByEmailAsync(string email)
        {
            var userLoginData = await context.UserLoginDatas
                .Where(uld => uld.Email == email)
                .Include(u => u.UserAccount)
                .ThenInclude(ua => ua.Role)
                .ThenInclude(ur => ur!.Permissions)
                .FirstOrDefaultAsync();

            return userLoginData;
        }

        public async Task<UserLoginData?> GetAsync(int ID)
        {
            var userLoginData = await context.UserLoginDatas.FindAsync(ID);

            return userLoginData;
        }

        public async Task<UserLoginData?> GetFullUserDataAsync(int ID)
        {
            var userLoginData = await context.UserLoginDatas
                .Where(uld => uld.ID == ID)
                .Include(u => u.UserAccount)
                .ThenInclude(ua => ua.Role)
                .ThenInclude(ur => ur!.Permissions)
                .FirstOrDefaultAsync();

            return userLoginData;
        }

        public async Task<UserLoginData?> GetByVerificationToken(string verificationToken)
        {
            return await context.UserLoginDatas
                .Where(uld => uld.VerificationToken == verificationToken)
                .FirstOrDefaultAsync();
        }
    }
}
