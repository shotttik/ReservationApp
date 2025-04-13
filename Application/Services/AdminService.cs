using Application.Authentication;
using Application.Common.ResultsErrors;
using Application.Common.ResultsErrors.User;
using Application.DTOs.Admin;
using Application.Extensions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Shared.Utilities;

namespace Application.Services
{
    public class AdminService :IAdminService
    {
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IAuthService authService;
        private readonly ICacheService cacheService;

        public AdminService(
            IUserLoginDataRepository userLoginDataRepository,
            IUserAccountRepository userAccountRepository,
            IRoleRepository roleRepository,
            IAuthService authService,
            ICacheService cacheService
            )
        {
            this.userLoginDataRepository = userLoginDataRepository;
            this.userAccountRepository = userAccountRepository;
            this.roleRepository = roleRepository;
            this.authService = authService;
            this.cacheService = cacheService;
        }

        public async Task<Result> AddUser(AddUserRequest request)
        {
            if (await userLoginDataRepository.GetByEmail(request.Email) is not null)
            {
                return Result.Failure(UserAddErrors.AlreadyExists);
            }

            // Validate roles first

            var r = await roleRepository.GetRole(request.Role);
            if (r is null)
            {
                return Result.Failure(UserAddErrors.RoleNotFound);
            }

            // Create user account and login data
            var userAccount = new UserAccount()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = (int)request.Gender,
                DateOfBirth = request.DateOfBirth,
                RoleID = request.Role,
                Role = r
            };

            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);

            userAccount = await userAccountRepository.Add(userAccount);
            var userLoginData = new UserLoginData()
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                UserAccountID = userAccount.ID
            };

            await userLoginDataRepository.Add(userLoginData);

            return Result.Success();
        }

        public async Task<Result> UpdateUser(UpdateUserRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();
            if (request == null)
            {
                return Result.Failure(UserUpdateErrors.ArgumentNull);
            }

            var userAccount = await userAccountRepository.Get((int)request.UserAccountID!);
            if (userAccount is null)
            {
                return Result.Failure(UserUpdateErrors.NotFound);
            }

            if (request.RoleID != null)
            {
                var r = await roleRepository.GetRole((int)request.RoleID);
                if (r is null)
                {
                    return Result.Failure(UserUpdateErrors.RoleNotFound);
                }
                userAccount.RoleID = r.ID;
            }

            if (AuthUser.Role.ID == Role.Admin.ID && userAccount.RoleID == Role.SuperAdmin.ID)
            {
                return Result.Failure(UserUpdateErrors.PermissionError);
            }

            userAccount.FirstName = request.FirstName ?? userAccount.FirstName;
            userAccount.LastName = request.LastName ?? userAccount.LastName;
            userAccount.Gender = request.Gender.HasValue ? (int)request.Gender.Value : userAccount.Gender;
            userAccount.DateOfBirth = request.DateOfBirth ?? userAccount.DateOfBirth;
            userAccount.UpdateTimestamp();
            await userAccountRepository.Update(userAccount);
            await cacheService.SetAsync(CacheUtils.AuthorizationCacheKey(userAccount.ID), userAccount.MapToAuthorizationData());

            return Result.Success();
        }
    }
}
