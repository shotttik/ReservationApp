using Application.Authentication;
using Application.Common.Requests;
using Application.Common.Requests.Admin;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Extensions.Mappers.Pagination;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO.User;
using Domain.Entities.User;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class AdminService :IAdminService
    {
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IAuthService authService;
        private readonly ICompanyRepository companyRepository;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;

        public AdminService(
            IUserLoginDataRepository userLoginDataRepository,
            IUserAccountRepository userAccountRepository,
            IAuthService authService,
            ICompanyRepository companyRepository,
            IConfiguration configuration,
            IUserService userService
            )
        {
            this.userLoginDataRepository = userLoginDataRepository;
            this.userAccountRepository = userAccountRepository;
            this.authService = authService;
            this.companyRepository = companyRepository;
            this.configuration = configuration;
            this.userService = userService;
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
                if (request.Role != Domain.Enums.Role.CompanyAdmin && request.Role != Domain.Enums.Role.CompanyEmployee)
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

            var userLoginData = new UserLoginData()
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                EmailVerificationStatus = VerificationStatus.Verified,
                UserAccount = userAccount,
            };

            await userLoginDataRepository.Add(userLoginData);

            return Result.Success(AuthResults.UserCreated);
        }

        public async Task<Result> UserUpdate(int id, UserUpdateRequest request)
        {
            var userAccount = await userAccountRepository.GetByUserLoginDataID(id);
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
            await authService.RefreshUserCache(id);

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

        public async Task<Result> CompanyUpdate(int id, CompanyUpdateRequest request)
        {
            var existingCompany = await companyRepository.GetWithLocation(id);
            if (existingCompany == null)
            {
                return Result.Failure(CompanyResults.CompanyDoesNotExists);
            }
            if (await companyRepository.ExistsByDetailsAsync(request.IN, request.Name, request.Email, request.Phone, excludeId: id))
                return Result.Failure(CompanyResults.AlreadyExists);

            var company = request.MapToEntity(existingCompany);

            await companyRepository.Update(company);

            return Result.Success(CompanyResults.Updated);
        }

        public async Task<Result<PagedList<UserLoginDataDTO>>> RetrievePagedUsers(PagedParameters parameters, CancellationToken cancellationToken)
        {
            var AuthUser = await authService.GetCurrentUser();
            var allowedFields = UserLoginDataFieldMap.DtoToEntityPath;
            var errors = parameters.Validate(allowedFields, typeof(UserLoginData));
            if (errors.Any())
            {
                return Result.Failure<PagedList<UserLoginDataDTO>>(PagedListResults.InvalidPagedParameters(errors.First()));
            }
            var mappedParams = PagedParameterMapper.MapToEntityPaths(parameters, allowedFields);
            var users = await userLoginDataRepository.RetrievePaged(mappedParams, cancellationToken, AuthUser.ID);

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

        public async Task<Result<UserLoginDataDTO>> GetUser(int id)
        {
            var user = await userLoginDataRepository.GetFullUserData(id);
            if (user is null)
            {
                return Result.Failure<UserLoginDataDTO>(AuthResults.UserNotFound);
            }
            return Result.Success(user.MapToDTO());
        }

        public async Task<Result> ChangeUserActiveStatus(ChangeStatusRequest request, int userId)
        {
            var userLoginData = await userLoginDataRepository.Get(userId);
            if (userLoginData == null)
            {
                return Result.Failure(AuthResults.UserNotFound);
            }
            if (userLoginData.ActiveStatus == request.NewStatus)
            {
                return Result.Failure(GenericResults.SameStatus);
            }
            if (request.NewStatus == ActiveStatus.Active)
            {
                userLoginData.Activate();
            }
            else
            {
                userLoginData.Disable();
                await userService.DeleteAllActiveSessions(userId);
            }
            await userLoginDataRepository.Update(userLoginData);

            return Result.Success(GenericResults.StatusChanged);
        }
    }
}
