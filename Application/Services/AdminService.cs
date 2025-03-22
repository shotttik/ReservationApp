using Application.Authentication;
using Application.Common.ResultsErrors;
using Application.Common.ResultsErrors.User;
using Application.DTOs.Admin;
using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Services
{
    public class AdminService :IAdminService
    {
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRoleRepository roleRepository;
        private readonly IDistributedCache cache;
        private readonly IUserService userService;

        public AdminService(
            IUserLoginDataRepository userLoginDataRepository,
            IUserAccountRepository userAccountRepository,
            IHttpContextAccessor httpContextAccessor,
            IRoleRepository roleRepository,
            IDistributedCache cache,
            IUserService userService
        )
        {
            this.userLoginDataRepository = userLoginDataRepository;
            this.userAccountRepository = userAccountRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.roleRepository = roleRepository;
            this.cache = cache;
            this.userService = userService;
        }

        public async Task<Result> AddUser(AddUserRequest request)
        {
            if (await userLoginDataRepository.GetByEmailAsync(request.Email) is not null)
            {
                return Result.Failure(UserAddErrors.AlreadyExists);
            }

            // Validate roles first

            var r = await roleRepository.GetRole(request.Role);
            if (r is null)
            {
                return Result.Failure(UserUpdateErrors.RoleNotFound);
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

            var userLoginData = new UserLoginData()
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                UserAccountID = await userAccountRepository.AddAsync(userAccount)
            };

            await userLoginDataRepository.AddAsync(userLoginData);

            return Result.Success();
        }

        public async Task<Result> UpdateUser(UpdateUserRequest request)
        {
            if (request == null)
            {
                return Result.Failure(UserUpdateErrors.ArgumentNull);
            }

            var authUserEmail = httpContextAccessor.HttpContext?.Items ["Email"] as string;
            if (authUserEmail == null)
                return Result.Failure<UserAccountDTO>(AuthorizationDataErrors.NotFound);

            var authUserLoginData = await userLoginDataRepository.GetFullUserDataByEmailAsync(authUserEmail);
            if (authUserLoginData == null)
                return Result.Failure<UserAccountDTO>(AuthorizationDataErrors.NotFound);

            var userAccount = await userAccountRepository.GetUserAccountByID((int)request.UserAccountID!);
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

            userAccount.FirstName = request.FirstName ?? userAccount.FirstName;
            userAccount.LastName = request.LastName ?? userAccount.LastName;
            userAccount.Gender = request.Gender.HasValue ? (int)request.Gender.Value : userAccount.Gender;
            userAccount.DateOfBirth = request.DateOfBirth ?? userAccount.DateOfBirth;


            await userAccountRepository.UpdateUserAccount(userAccount);
            await cache.RemoveAsync(GetCacheKey(userAccount.ID));
            await userService.GetUserAuthorizationDataAsync();

            return Result.Success();
        }
        private string GetCacheKey(int userID) => $"UserAuthorization:{userID}";

    }
}
