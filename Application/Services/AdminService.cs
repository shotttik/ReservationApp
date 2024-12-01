using Application.Common.ResultsErrors;
using Application.Common.ResultsErrors.User;
using Application.DTOs.User;
using Application.Enums;
using Application.Helpers;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class AdminService :IAdminService
    {
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AdminService(
            IUserLoginDataRepository userLoginDataRepository,
            IUserRoleRepository userRoleRepository,
            IUserAccountRepository userAccountRepository,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this.userLoginDataRepository = userLoginDataRepository;
            this.userRoleRepository = userRoleRepository;
            this.userAccountRepository = userAccountRepository;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result> AddUser(AddRequest request)
        {
            if (await userLoginDataRepository.GetByEmailAsync(request.Email) is not null)
            {
                return Result.Failure(UserAddErrors.AlreadyExists);
            }
            var role = await userRoleRepository.GetUserRole(((int)request.Role));
            if (role == null)
            {
                return Result.Failure(UserAddErrors.RoleNotFound);
            }

            var userAccount = new UserAccount()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = (int)request.Gender,
                DateOfBirth = request.DateOfBirth,
                RoleID = role.ID
            };

            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);

            var userLoginData = new UserLoginData()
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
            };
            var userAccountID = await userAccountRepository.AddAsync(userAccount);
            userLoginData.UserAccountID = userAccountID;
            await userLoginDataRepository.AddAsync(userLoginData);

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
            var updateUserIsAdmin = Enum.Parse(typeof(Role), userAccount.Role.RoleDescription) is Role.Admin;
            var authUserIsSuperAdmin = Enum.Parse(typeof(Role), authUserLoginData.UserAccount.Role.RoleDescription) is Role.SuperAdmin;
            if (updateUserIsAdmin && !authUserIsSuperAdmin)
            {
                return Result.Failure(UserUpdateErrors.PermissionError);
            }
            userAccount.FirstName = request.FirstName ?? userAccount.FirstName;
            userAccount.LastName = request.LastName ?? userAccount.LastName;
            userAccount.Gender = request.Gender.HasValue ? (int)request.Gender.Value : userAccount.Gender;
            userAccount.DateOfBirth ??= request.DateOfBirth;
            userAccount.DateOfBirth = request.DateOfBirth ?? userAccount.DateOfBirth;
            userAccount.RoleID = request.Role.HasValue ? (int)request.Role.Value : userAccount.RoleID;
            await userAccountRepository.UpdateUserAccount(userAccount);

            return Result.Success();
        }
    }
}
