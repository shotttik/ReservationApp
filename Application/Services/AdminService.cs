using Application.Authentication;
using Application.Common.ResultsErrors;
using Application.Common.ResultsErrors.User;
using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class AdminService :IAdminService
    {
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRoleRepository roleRepository;

        public AdminService(
            IUserLoginDataRepository userLoginDataRepository,
            IUserAccountRepository userAccountRepository,
            IHttpContextAccessor httpContextAccessor,
            IRoleRepository roleRepository
        )
        {
            this.userLoginDataRepository = userLoginDataRepository;
            this.userAccountRepository = userAccountRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.roleRepository = roleRepository;
        }

        public async Task<Result> AddUser(AddRequest request)
        {
            if (await userLoginDataRepository.GetByEmailAsync(request.Email) is not null)
            {
                return Result.Failure(UserAddErrors.AlreadyExists);
            }
            if (request.Roles == null || request.Roles.Count == 0)
            {
                return Result.Failure(UserUpdateErrors.RolesEmpty);
            }

            // Validate roles first
            var roles = new List<Role>();
            foreach (var role in request.Roles)
            {
                var r = await roleRepository.GetRole(role);
                if (r is null)
                {
                    return Result.Failure(UserUpdateErrors.RoleNotFound);
                }
                roles.Add(r);
            }

            // Create user account and login data
            var userAccount = new UserAccount()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = (int)request.Gender,
                DateOfBirth = request.DateOfBirth,
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

            // Assign roles to user account
            userAccount.Roles = roles;

            await userAccountRepository.UpdateUserAccount(userAccount);

            return Result.Success();
        }

        public async Task<Result> UpdateUser(UpdateRequest request)
        {
            if (request == null)
            {
                return Result.Failure(UserUpdateErrors.ArgumentNull);
            }

            var authUserEmail = httpContextAccessor.HttpContext?.Items ["Email"] as string;
            if (authUserEmail == null)
                return Result.Failure<UserAccountDTO>(AuthorizationDataErrors.NotFound);

            var authUserLoginData = await userLoginDataRepository.GetByEmailAsync(authUserEmail);
            if (authUserLoginData == null)
                return Result.Failure<UserAccountDTO>(AuthorizationDataErrors.NotFound);

            var userAccount = await userAccountRepository.GetUserAccountByID(request.UserAccountID);
            if (userAccount is null)
            {
                return Result.Failure(UserUpdateErrors.NotFound);
            }

            // Validate roles first
            var roles = new List<Role>();
            if (request.Roles != null && request.Roles.Count > 0)
            {
                foreach (var role in request.Roles)
                {
                    var r = await roleRepository.GetRole(role);
                    if (r is null)
                    {
                        return Result.Failure(UserUpdateErrors.RoleNotFound);
                    }
                    roles.Add(r);
                }
            }

            userAccount.FirstName = request.FirstName ?? userAccount.FirstName;
            userAccount.LastName = request.LastName ?? userAccount.LastName;
            userAccount.Gender = request.Gender.HasValue ? (int)request.Gender.Value : userAccount.Gender;
            userAccount.DateOfBirth = request.DateOfBirth ?? userAccount.DateOfBirth;

            // Assign roles to user account
            userAccount.Roles.Clear();
            userAccount.Roles = roles;

            await userAccountRepository.UpdateUserAccount(userAccount);

            return Result.Success();
        }
    }
}
