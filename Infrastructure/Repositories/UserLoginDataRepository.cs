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
        public async Task<UserLoginData?> GetFullUserDataByEmail(string email)
        {
            var userLoginData = await _dbSet
                .Where(uld => uld.Email == email)
                .Include(u => u.UserAccount)
                    .ThenInclude(ua => ua.Role)
                        .ThenInclude(ur => ur!.Permissions)
                .Include(u => u.UserAccount)
                    .ThenInclude(e => e.Company)
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
