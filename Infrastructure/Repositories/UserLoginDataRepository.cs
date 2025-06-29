using Application.Extensions.Mappers;
using Domain.Abstractions;
using Domain.DTO.User;
using Domain.Entities.User;
using Domain.Interfaces.Repositories;
using Infrastructure.Extensions;
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
                     .ThenInclude(u => u.WorkSchedules)
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
                .Include(u => u.UserAccount)
                   .ThenInclude(u => u.WorkSchedules)
                .FirstOrDefaultAsync();

            return userLoginData;
        }

        public async Task<UserLoginData?> GetByVerificationToken(string verificationToken)
        {
            return await _dbSet
                .Where(uld => uld.VerificationToken == verificationToken)
                .FirstOrDefaultAsync();
        }
        public async Task<UserLoginData?> GetByUserAccountID(int userAccountID)
        {
            return await _dbSet
                .Where(uld => uld.UserAccountID == userAccountID)
                .FirstOrDefaultAsync();
        }

        public async Task<UserLoginData?> GetByRecoveryToken(string recoveryToken)
        {
            return await _dbSet.
                Where(uld => uld.RecoveryToken == recoveryToken)
                .FirstOrDefaultAsync();
        }
        public async Task<PagedList<UserLoginDataDTO>> RetrievePaged(
           PagedParameters parameters,
           CancellationToken cancellationToken,
           int authUserID)
        {
            var query = _dbSet.AsQueryable();

            query = query.ApplyQueryParamsAsync(parameters);

            var totalCount = await query.CountAsync();

            var users = await query
                .Include(u => u.UserAccount)
                    .ThenInclude(ua => ua.Role)
                        .ThenInclude(ur => ur!.Permissions)
                .Include(u => u.UserAccount)
                    .ThenInclude(e => e.Company)
                        .ThenInclude(c => c.WorkSchedules)
                .Where(u => u.ID != authUserID)
                .Select(e => e.MapToDTO())
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<UserLoginDataDTO>(users, parameters.PageNumber, parameters.PageSize, totalCount);
        }
    }
}
