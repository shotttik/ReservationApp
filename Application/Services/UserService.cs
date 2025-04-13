using Application.Authentication;
using Application.Common.ResultsErrors;
using Application.Common.ResultsErrors.User;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Extensions;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Utilities;
using System.Security.Claims;

namespace Application.Services
{
    public class UserService :IUserService
    {
        private readonly IConfiguration configuration;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly IRoleRepository roleRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IAuthService authService;
        private readonly ICacheService cacheService;

        public UserService(
            IConfiguration configuration,
            IUserAccountRepository userAccountRepository,
            IUserLoginDataRepository userLoginDataRepository,
            IRoleRepository roleRepository,
            ICompanyRepository companyRepository,
            IHttpContextAccessor httpContextAccessor,
            IAuthService authService,
            ICacheService cacheService)
        {
            this.configuration = configuration;
            this.userAccountRepository = userAccountRepository;
            this.userLoginDataRepository = userLoginDataRepository;
            this.roleRepository = roleRepository;
            this.companyRepository = companyRepository;
            this.authService = authService;
            this.cacheService = cacheService;
        }

        public async Task<Result> Register(RegisterUserRequest request)
        {
            if (await userLoginDataRepository.GetByEmail(request.Email) != null)
            {
                return Result.Failure(RegisterErrors.AlreadyExists);
            }
            var role = await roleRepository.GetRole(request.RoleID);
            if (role is null)
            {
                return Result.Failure(RegisterErrors.RoleNotFound);
            }
            if (!(role.ID == Role.User.ID || role.ID == Role.CompanyAdmin.ID))
            {
                return Result.Failure(RegisterErrors.RoleIsNotAccessable);
            }
            if ((role.ID == Role.User.ID && request.Company != null) ||
                (role.ID) == Role.CompanyAdmin.ID && request.Company == null)
            {
                return Result.Failure(RegisterErrors.RoleIncompatibility);
            }

            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);
            var verificationToken = JWTGenerator.GenerateAndHashSecureToken();
            var expDays = Convert.ToDouble(configuration ["Jwt:VerificationTokenExpirationDays"]);
            var verificationTokenExpirationTime = DateTime.Now.AddDays(expDays);

            var userAccount = new UserAccount
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                RoleID = role.ID
            };

            var userLoginData = new UserLoginData
            {
                Email = request.Email,
                VerificationStatus = VerificationStatus.Pending,
                PasswordHash = hash,
                PasswordSalt = salt,
                VerificationToken = verificationToken,
                VerificationTokenExpTime = verificationTokenExpirationTime
            };

            if (request.Company != null)
            {
                var company = new Company()
                {
                    Name = request.Company.Name,
                    Description = request.Company.Description,
                    IN = request.Company.IN,
                    Email = request.Company.Email,
                    Phone = request.Company.Phone
                };
                await companyRepository.Add(company);
            }

            userAccount = await userAccountRepository.Add(userAccount);
            userLoginData.UserAccountID = userAccount.ID;
            await userLoginDataRepository.Add(userLoginData);

            var response = new RegisterResponse()
            {
                Description = $"User registered successfully, Now You have to Verify your email, check inbox, you have {expDays} days.",
                VerificationToken = verificationToken,
                VerificationTokenExpTime = verificationTokenExpirationTime
            };

            return Result.Success(response);
        }
        public async Task<Result<LoginResponse>> Login(LoginRequest request)
        {
            var user = await userLoginDataRepository.GetFullUserDataByEmail(request.Email);

            if (user == null)
            {
                return Result.Failure<LoginResponse>(LoginErrors.NotFound);
            }
            if (user.VerificationStatus != VerificationStatus.Verified)
            {
                return Result.Failure<LoginResponse>(LoginErrors.EmailNotVerified);
            }
            if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Result.Failure<LoginResponse>(LoginErrors.InvalidPassword);
            }
            var accessToken = JWTGenerator.GenerateAccessToken(user.ID, user.UserAccountID, user.Email, configuration);
            var refreshToken = JWTGenerator.GenerateAndHashSecureToken();

            var refreshTokenExpirationTime = DateTime.Now.AddDays(Convert.ToDouble(configuration ["Jwt:RefreshTokenExpirationDays"]));
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpTime = refreshTokenExpirationTime;
            user.UpdateTimestamp();

            var userDTO = user.UserAccount.MapToAuthorizationData();
            await cacheService.SetAsync(CacheUtils.AuthorizationCacheKey(user.UserAccountID), userDTO);
            await userLoginDataRepository.Update(user);

            return Result.Success(new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpTime = DateTime.Now.AddMinutes(Convert.ToDouble(configuration ["Jwt:AccessTokenExpirationMinutes"])),
            });
        }
        public async Task<Result<RefreshResponse>> Refresh(TokenRequest request)
        {
            var principal = JWTGenerator.GetPrincipalFromExpiredToken(request.AccessToken, configuration);
            if (principal == null)
            {
                Result.Failure<RefreshResponse>(RefreshTokenErrors.InvalidToken);
            }
            var email = principal!.FindFirst(ClaimTypes.Email)?.Value!;
            if (email.IsNullOrEmpty())
            {
                return Result.Failure<RefreshResponse>(RefreshTokenErrors.InvalidToken);
            }
            var user = await userLoginDataRepository.GetFullUserDataByEmail(email);
            if (user is null)
            {
                return Result.Failure<RefreshResponse>(RefreshTokenErrors.NotFound);
            };
            if (user.RefreshToken is null ||
                user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpTime < DateTime.Now)
            {
                return Result.Failure<RefreshResponse>(RefreshTokenErrors.InvalidToken);
            }

            var newAccessToken = JWTGenerator.GenerateAccessToken(user.ID, user.UserAccountID, email, configuration);
            var newRefreshToken = JWTGenerator.GenerateAndHashSecureToken();

            var refreshTokenExpirationTime = DateTime.Now.AddDays(Convert.ToDouble(configuration ["Jwt:RefreshTokenExpirationDays"]));
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpTime = refreshTokenExpirationTime;
            user.UpdateTimestamp();
            await userLoginDataRepository.Update(user);

            var response = new RefreshResponse()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return Result.Success(response);

        }
        public async Task<Result> Logout()
        {
            UserAccountDTO AuthUser;
            try
            {
                AuthUser = await authService.GetCurrentUser();
            }
            catch (AuthorizationException)
            {

                return Result.Failure<UserAccountDTO>(AuthorizationDataErrors.NotFound);
            }
            var userLoginData = await userLoginDataRepository.GetByUserAccountID(AuthUser.ID);

            if (userLoginData is null)
            {
                return Result.Failure(LogoutErrors.NotFound);
            }
            userLoginData.RefreshToken = null;
            userLoginData.RefreshTokenExpTime = null;
            userLoginData.UpdateTimestamp();

            await userLoginDataRepository.Update(userLoginData);
            await cacheService.RemoveAsync(CacheUtils.AuthorizationCacheKey(AuthUser.ID));

            return Result.Success();
        }
        public async Task<Result<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            var userLoginData = await userLoginDataRepository.GetByEmail(request.Email);
            if (userLoginData is null)
            {
                return Result.Failure<string>(ForgotPasswordErrors.NotFound);
            }
            var recoveryToken = JWTGenerator.GenerateAndHashSecureToken();
            var recoveryTokenTime = DateTime.Now.AddMinutes(Convert.ToDouble(configuration ["Jwt:RecoveryTokenExpirationMinutes"]));
            userLoginData.RecoveryToken = recoveryToken;
            userLoginData.RecoveryTokenExpTime = recoveryTokenTime;
            userLoginData.UpdateTimestamp();
            await userLoginDataRepository.Update(userLoginData);

            // TODO instead of returning recovery token need to send email to user
            return Result.Success(recoveryToken);
        }
        public async Task<Result> ResetPassword(ResetPasswordRequest request)
        {
            var userLoginData = await userLoginDataRepository.GetByEmail(request.Email);

            if (userLoginData is null)
            {
                return Result.Failure(ResetPasswordErrors.NotFound);
            }
            if (userLoginData.RecoveryToken is null ||
                userLoginData.RecoveryToken != request.RecoveryToken ||
                userLoginData.RecoveryTokenExpTime < DateTime.Now)
            {
                return Result.Failure(ResetPasswordErrors.InvalidToken);
            }

            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);
            userLoginData!.PasswordHash = hash;
            userLoginData.PasswordSalt = salt;
            userLoginData.RecoveryToken = null;
            userLoginData.RecoveryTokenExpTime = null;
            userLoginData.UpdateTimestamp();

            await userLoginDataRepository.Update(userLoginData);

            return Result.Success();
        }
        public async Task<Result<UserAccountDTO>> GetUserAuthorizationDataAsync()
        {
            try
            {
                var AuthUser = await authService.GetCurrentUser();

                return Result.Success(AuthUser);
            }
            catch (AuthorizationException)
            {

                return Result.Failure<UserAccountDTO>(AuthorizationDataErrors.NotFound);
            }
        }
        public async Task<Result> VerifyEmail(string token)
        {
            var userLoginData = await userLoginDataRepository.GetByVerificationToken(token);
            if (userLoginData is null)
            {
                return Result.Failure(VerifyEmailErrors.NotFound);
            }
            if (userLoginData.VerificationTokenExpTime < DateTime.Now)
            {
                return Result.Failure(VerifyEmailErrors.ExpiredToken);
            }

            userLoginData.VerificationToken = null;
            userLoginData.VerificationTokenExpTime = null;
            userLoginData.VerificationStatus = VerificationStatus.Verified;
            userLoginData.UpdateTimestamp();
            await userLoginDataRepository.Update(userLoginData);

            return Result.Success();
        }
    }
}
