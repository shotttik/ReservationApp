using Application.Authentication;
using Application.Common.Requests;
using Application.Common.Requests.Admin;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Extensions.Mappers.Pagination;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO.User;
using Domain.Entities.CompanyReleated;
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
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

        public AdminService(
            IUserLoginDataRepository userLoginDataRepository,
            IUserAccountRepository userAccountRepository,
            IAuthService authService,
            ICompanyRepository companyRepository,
            IConfiguration configuration,
            IUserService userService,
            ISubscriptionPlanRepository subscriptionPlanRepository
            )
        {
            this.userLoginDataRepository = userLoginDataRepository;
            this.userAccountRepository = userAccountRepository;
            this.authService = authService;
            this.companyRepository = companyRepository;
            this.configuration = configuration;
            this.userService = userService;
            _subscriptionPlanRepository = subscriptionPlanRepository;
        }

        public async Task<Result> UserCreate(UserCreateRequest request)
        {
            if (await userLoginDataRepository.GetByEmail(request.Email) is not null)
            {
                return Result.Failure(AuthResults.EmailAlreadyExists);
            }
            if (request.CompanyId.HasValue)
            {
                var company = await companyRepository.GetWithBranches(request.CompanyId.Value);
                if (company is null)
                {
                    return Result.Failure(CompanyResults.CompanyDoesNotExists);
                }
                if (request.BranchId is not null && !company.HasBranch((int)request.BranchId))
                {
                    return Result.Failure(CompanyResults.InvalidBranchId);
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
                CompanyID = request.CompanyId,
                BranchId = request.BranchId
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

            if (request.Role.HasValue)
            {
                if (request.CompanyId.HasValue)
                {
                    var company = await companyRepository.GetWithBranches(request.CompanyId.Value);
                    if (company is null)
                    {
                        return Result.Failure(CompanyResults.CompanyDoesNotExists);
                    }
                    if (request.BranchId is not null && !company.HasBranch((int)request.BranchId))
                    {
                        return Result.Failure(CompanyResults.InvalidBranchId);
                    }
                }
                userAccount.RoleID = (int)request.Role;
                userAccount.CompanyID = request.CompanyId;
                userAccount.BranchId = request.BranchId;
            }
            if (request.FirstName is not null) userAccount.FirstName = request.FirstName;
            if (request.LastName is not null) userAccount.LastName = request.LastName;
            if (request.Gender.HasValue) userAccount.Gender = request.Gender.Value;
            if (request.DateOfBirth.HasValue) userAccount.DateOfBirth = request.DateOfBirth.Value;
            if (request.ActiveStatus is not null)
            {
                if (request.ActiveStatus == ActiveStatus.Active)
                {
                    userAccount.UserLoginData.Activate();
                }
                else
                {
                    userAccount.UserLoginData.Disable();
                    await userService.DeleteAllActiveSessions(id);
                }
            }
            await userAccountRepository.Update(userAccount);
            await authService.RefreshUserCache(id);

            return Result.Success(AuthResults.UserUpdated);
        }

        public async Task<Result> ResetUserPassword(int id, AdminResetPasswordRequest request)
        {
            var userLoginData = await userLoginDataRepository.Get(id);
            if (userLoginData is null)
            {
                return Result.Failure(AuthResults.UserNotFound);
            }
            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);

            userLoginData!.PasswordHash = hash;
            userLoginData.PasswordSalt = salt;

            await userLoginDataRepository.Update(userLoginData);

            return Result.Success(AuthResults.PasswordReseted);
        }

        public async Task<Result> CompanyCreate(CompanyCreateRequest request)
        {
            if (await companyRepository.ExistsByDetailsAsync(request.IN, request.Name, request.Email, request.Phone))
            {
                return Result.Failure(CompanyResults.AlreadyExists);
            }
            var subscriptionPlan = await _subscriptionPlanRepository.Get(request.SubscriptionPlanId);
            if (subscriptionPlan == null)
            {
                return Result.Failure(SubscriptionPlanResults.DoesntExists);
            }

            var company = request.MapToEntity();

            var companySubscription = new CompanySubscription()
            {
                SubscriptionPlanId = subscriptionPlan.ID,
                SubscriptionPlan = subscriptionPlan,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                Status = SubscriptionStatus.Active,
                AutoRenew = false
            };
            company.Subscription = companySubscription;
            await companyRepository.Add(company);

            return Result.Success();
        }

        public async Task<Result> CompanyUpdate(int id, CompanyUpdateRequest request)
        {
            var company = await companyRepository.GetWithBranches(id);
            if (company == null)
            {
                return Result.Failure(CompanyResults.CompanyDoesNotExists);
            }
            if (await companyRepository.ExistsByDetailsAsync(request.IN, request.Name, request.Email, request.Phone, excludeId: id))
                return Result.Failure(CompanyResults.AlreadyExists);

            request.MapToEntity(company);

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
            var users = await userLoginDataRepository.RetrievePaged(mappedParams, cancellationToken, AuthUser.Id);

            return Result.Success(users);
        }
        public async Task<Result> AssignUserToCompany(AssignUserToCompanyRequest request)
        {
            var user = await userAccountRepository.GetByUserLoginDataID(request.UserId);
            if (user is null)
            {
                return Result.Failure(AuthResults.UserDoesntExists);
            }

            var company = await companyRepository.GetWithBranches(request.CompanyId);
            if (company is null)
            {
                return Result.Failure(CompanyResults.CompanyDoesNotExists);
            }
            if (request.IsRoleCompanyEmployee && !company.HasBranch((int)request.BranchId!)) // tu companyemployee aris mashin branchid null arikneba
            {
                return Result.Failure(CompanyResults.InvalidBranchId);
            }
            user.CompanyID = request.CompanyId;
            user.RoleID = (int)request.Role;
            user.BranchId = request.IsRoleCompanyEmployee ? request.BranchId : null; // only employee assigned to branch
            await userAccountRepository.Update(user);
            await authService.RefreshUserCache(request.UserId);

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
    }
}
