using Application.Authentication;
using Application.Common.Results;
using Application.DTOs.Admin;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
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
        private readonly ICompanyRepository companyRepository;

        public AdminService(
            IUserLoginDataRepository userLoginDataRepository,
            IUserAccountRepository userAccountRepository,
            IRoleRepository roleRepository,
            IAuthService authService,
            ICacheService cacheService,
            ICompanyRepository companyRepository
            )
        {
            this.userLoginDataRepository = userLoginDataRepository;
            this.userAccountRepository = userAccountRepository;
            this.roleRepository = roleRepository;
            this.authService = authService;
            this.cacheService = cacheService;
            this.companyRepository = companyRepository;
        }

        public async Task<Result> UserCreate(UserCreateRequest request)
        {
            if (await userLoginDataRepository.GetByEmail(request.Email) is not null)
            {
                return Result.Failure(AuthResults.EmailAlreadyExists);
            }

            // Create user account and login data
            var userAccount = new UserAccount()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = (int)request.Gender,
                DateOfBirth = request.DateOfBirth,
                RoleID = Role.FromID((int)request.Role)!.ID,
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

        public async Task<Result> UserUpdate(UserUpdateRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();
            if (request == null)
            {
                return Result.Failure(AuthResults.ArgumentNull);
            }

            var userAccount = await userAccountRepository.Get(request.UserAccountID);
            if (userAccount is null)
            {
                return Result.Failure(AuthResults.UserNotFound);
            }

            if (request.Role != null)
            {
                var r = Role.FromID((int)request.Role);
                if (r is null)
                {
                    return Result.Failure(AuthResults.RoleNotFound);
                }
                userAccount.RoleID = r.ID;
            }

            userAccount.FirstName = request.FirstName ?? userAccount.FirstName;
            userAccount.LastName = request.LastName ?? userAccount.LastName;
            userAccount.Gender = request.Gender.HasValue ? (int)request.Gender.Value : userAccount.Gender;
            userAccount.DateOfBirth = request.DateOfBirth ?? userAccount.DateOfBirth;
            await userAccountRepository.Update(userAccount);
            await cacheService.SetAsync(CacheUtils.AuthorizationCacheKey(userAccount.ID), userAccount.MapToAuthorizationData());

            return Result.Success();
        }

        public async Task<Result> CompanyCreate(CompanyCreateRequest request)
        {
            if (await companyRepository.ExistsByDetailsAsync(request.IN, request.Name, request.Email, request.Phone))
            {
                return Result.Failure(CompanyResults.AlreadyExists);
            }

            var company = new Company()
            {
                Name = request.Name,
                Description = request.Description,
                IN = request.IN,
                Email = request.Email,
                Phone = request.Phone
            };
            await companyRepository.Add(company);

            return Result.Success();
        }
    }
}
