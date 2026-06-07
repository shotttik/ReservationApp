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
               .Include(u => u.UserAccount)
                    .ThenInclude(ua => ua.UserAccountMedia)
                        .ThenInclude(uam => uam.Media)
               .Include(u => u.UserAccount)
                    .ThenInclude(ua => ua.EmployeeServices)
                        .ThenInclude(es => es.Service)
                .FirstOrDefaultAsync();

            return userLoginData;
        }
        public async Task<UserLoginData?> GetFullUserData(int ID)
        {
            var userLoginData = await _dbSet
                .Where(uld => uld.Id == ID)
                .Include(u => u.UserAccount)
                    .ThenInclude(ua => ua.Role)
                        .ThenInclude(ur => ur!.Permissions)
                .Include(u => u.UserAccount)
                   .ThenInclude(u => u.WorkSchedules)
                .Include(u => u.UserAccount)
                   .ThenInclude(u => u.UserAccountMedia)
                        .ThenInclude(uam => uam.Media)
                .Include(u => u.UserAccount)
                    .ThenInclude(ua => ua.EmployeeServices)
                        .ThenInclude(es => es.Service)
                .FirstOrDefaultAsync();

            return userLoginData;
        }

        public async Task<UserLoginData?> GetWithUserAccount(int ID, int companyID)
        {
            return await _dbSet
                .Where(uld => uld.Id == ID && uld.UserAccount.CompanyID == companyID)
                .Include(uld => uld.UserAccount)
                .FirstOrDefaultAsync();
        }

        public async Task<UserLoginData?> GetByEmailVerificationToken(string verificationToken)
        {
            return await _dbSet
                .Where(uld => uld.EmailVerificationToken == verificationToken)
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
            var query = _dbSet
            .Include(u => u.UserAccount)
                .ThenInclude(ua => ua.Role)
            .Where(u => u.Id != authUserID)
            .AsQueryable();

            query = query.ApplyQueryParamsAsync(parameters);

            var totalCount = await query.CountAsync(cancellationToken);

            var users = await query
                .Select(e => e.MapToDTO())
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<UserLoginDataDTO>(users, parameters.PageNumber, parameters.PageSize, totalCount);
        }
        public async Task<PagedList<UserLoginDataDTO>> RetrievePagedCompanyEmployees(
           PagedParameters parameters,
           CancellationToken cancellationToken,
           int authUserID,
           int companyID)
        {
            var query = _dbSet
            .Include(u => u.UserAccount)
                .ThenInclude(ua => ua.Role)
            .Include(u => u.UserAccount.EmployeeServices)
                .ThenInclude(u=> u.Service)
            .Where(u => u.Id != authUserID && u.UserAccount.CompanyID == companyID)
            .AsQueryable();

            query = query.ApplyQueryParamsAsync(parameters);

            var totalCount = await query.CountAsync(cancellationToken);

            var users = await query
                .Select(e => e.MapToDTO())
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<UserLoginDataDTO>(users, parameters.PageNumber, parameters.PageSize, totalCount);
        }
    }
}
