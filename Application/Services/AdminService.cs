using Application.Authentication;
using Application.Common.Requests.Admin;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO.User;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;

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
        private readonly IConfiguration configuration;

        public AdminService(
            IUserLoginDataRepository userLoginDataRepository,
            IUserAccountRepository userAccountRepository,
            IRoleRepository roleRepository,
            IAuthService authService,
            ICacheService cacheService,
            ICompanyRepository companyRepository,
            IConfiguration configuration
            )
        {
            this.userLoginDataRepository = userLoginDataRepository;
            this.userAccountRepository = userAccountRepository;
            this.roleRepository = roleRepository;
            this.authService = authService;
            this.cacheService = cacheService;
            this.companyRepository = companyRepository;
            this.configuration = configuration;
        }

        public async Task<Result> UserCreate(UserCreateRequest request)
        {
            if (await userLoginDataRepository.GetByEmail(request.Email) is not null)
            {
                return Result.Failure(AuthResults.EmailAlreadyExists);
            }
            // company - role compatibility check
            if (request.CompanyID.HasValue)
            {
                if (request.Role != Domain.Enums.Role.CompanyAdmin && request.Role != Domain.Enums.Role.CompanyMember)
                {
                    return Result.Failure(AuthResults.RoleIncompatibility);
                }
                var company = await companyRepository.Get(request.CompanyID.Value);
                if (company is null)
                {
                    return Result.Failure(CompanyResults.CompanyDoesNotExists);
                }
            }
            var verificationToken = JWTGenerator.GenerateAndHashSecureToken();
            var expDays = Convert.ToDouble(configuration ["Jwt:VerificationTokenExpirationDays"]);
            var verificationTokenExpirationTime = DateTime.UtcNow.AddDays(expDays);

            // Create user account and login data
            var userAccount = new UserAccount()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                RoleID = (int)request.Role,
                CompanyID = request.CompanyID
            };

            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);

            userAccount = await userAccountRepository.Add(userAccount);
            var userLoginData = new UserLoginData()
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                UserAccountID = userAccount.ID,
                VerificationToken = verificationToken,
                VerificationTokenExpTime = verificationTokenExpirationTime,
            };

            await userLoginDataRepository.Add(userLoginData);

            return Result.Success(AuthResults.UserCreated);
        }

        public async Task<Result> UserUpdate(UserUpdateRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();
            if (request == null)
            {
                return Result.Failure(AuthResults.ArgumentNull);
            }

            var userAccount = await userAccountRepository.GetByUserLoginDataID(request.ID);
            if (userAccount is null)
            {
                return Result.Failure(AuthResults.UserNotFound);
            }

            if (request.Role.HasValue) userAccount.RoleID = (int)request.Role;
            if (request.FirstName is not null) userAccount.FirstName = request.FirstName;
            if (request.LastName is not null) userAccount.LastName = request.LastName;
            if (request.Gender.HasValue) userAccount.Gender = request.Gender.Value;
            if (request.DateOfBirth.HasValue) userAccount.DateOfBirth = request.DateOfBirth.Value;
            await userAccountRepository.Update(userAccount);
            await authService.RefreshUserCache(request.ID);

            return Result.Success(AuthResults.UserUpdated);
        }

        public async Task<Result> CompanyCreate(CompanyCreateRequest request)
        {
            if (await companyRepository.ExistsByDetailsAsync(request.IN, request.Name, request.Email, request.Phone))
            {
                return Result.Failure(CompanyResults.AlreadyExists);
            }

            var company = request.MapToEntity();
            await companyRepository.Add(company);

            return Result.Success();
        }

        public async Task<Result<PagedList<UserLoginDataDTO>>> RetrievePagedUsers(PagedParameters parameters, CancellationToken cancellationToken)
        {
            var AuthUser = await authService.GetCurrentUser();
            var errors = parameters.Validate<AuthUser>();
            if (errors.Any())
            {
                return Result.Failure<PagedList<UserLoginDataDTO>>(PagedListResults.InvalidPagedParameters(errors.First()));
            }
            var users = await userLoginDataRepository.RetrievePaged(parameters, cancellationToken, AuthUser.ID);

            return Result.Success(users);
        }
        public async Task<Result> AssignUserToCompany(AssignUserToCompanyRequest request)
        {
            var user = await userAccountRepository.GetByUserLoginDataID(request.UserID);
            if (user is null)
            {
                return Result.Failure(AuthResults.UserDoesntExists);
            }
            if (!request.IsRoleValid)
            {
                return Result.Failure(AuthResults.RoleIncompatibility);
            }
            var company = await companyRepository.Get(request.CompanyID);
            if (company is null)
            {
                return Result.Failure(CompanyResults.CompanyDoesNotExists);
            }
            if (user.CompanyID == request.CompanyID && user.RoleID == (int)request.Role)
            {
                return Result.Failure(AuthResults.UserAlreadyAssignedToCompany);
            }
            user.CompanyID = request.CompanyID;
            user.RoleID = (int)request.Role;
            await userAccountRepository.Update(user);
            await authService.RefreshUserCache(request.UserID);

            return Result.Success(AuthResults.UserAssignedToCompany);
        }
    }
}
