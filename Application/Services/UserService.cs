using Application.Authentication;
using Application.Common.Requests.User;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Exceptions;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Utilities;
using System.Security.Claims;
using Role = Domain.Entities.Role;

namespace Application.Services
{
    public class UserService :IUserService
    {
        private readonly IConfiguration configuration;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly IAuthService authService;
        private readonly ICacheService cacheService;

        public UserService(
            IConfiguration configuration,
            IUserAccountRepository userAccountRepository,
            IUserLoginDataRepository userLoginDataRepository,
            IHttpContextAccessor httpContextAccessor,
            IAuthService authService,
            ICacheService cacheService)
        {
            this.configuration = configuration;
            this.userAccountRepository = userAccountRepository;
            this.userLoginDataRepository = userLoginDataRepository;
            this.authService = authService;
            this.cacheService = cacheService;
        }

        public async Task<Result<RegisterResponse>> Register(RegisterUserRequest request)
        {
            if (await userLoginDataRepository.GetByEmail(request.Email) != null)
            {
                return Result.Failure<RegisterResponse>(AuthResults.EmailAlreadyExists);
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
                RoleID = Role.PublicUser.ID,
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

            userAccount = await userAccountRepository.Add(userAccount);
            userLoginData.UserAccountID = userAccount.ID;
            await userLoginDataRepository.Add(userLoginData);

            var response = new RegisterResponse()
            {
                VerificationToken = verificationToken,
                VerificationTokenExpTime = verificationTokenExpirationTime
            };

            return Result.Success(response, AuthResults.Registered);
        }
        public async Task<Result<LoginResponse>> Login(LoginRequest request)
        {
            var user = await userLoginDataRepository.GetFullUserDataByEmail(request.Email);

            if (user == null)
            {
                return Result.Failure<LoginResponse>(AuthResults.UserNotFound);
            }
            if (user.VerificationStatus != VerificationStatus.Verified)
            {
                return Result.Failure<LoginResponse>(AuthResults.EmailNotVerified);
            }
            if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Result.Failure<LoginResponse>(AuthResults.InvalidPassword);
            }
            var accessToken = JWTGenerator.GenerateAccessToken(user.ID, user.UserAccountID, user.Email, configuration);
            var refreshToken = JWTGenerator.GenerateAndHashSecureToken();

            var refreshTokenExpirationTime = DateTime.Now.AddDays(Convert.ToDouble(configuration ["Jwt:RefreshTokenExpirationDays"]));
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpTime = refreshTokenExpirationTime;

            var userDTO = user.UserAccount.MapToAuthorizationData();
            await cacheService.SetAsync(CacheUtils.AuthorizationCacheKey(user.UserAccountID), userDTO);
            await userLoginDataRepository.Update(user);

            return Result.Success(new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpTime = DateTime.Now.AddMinutes(Convert.ToDouble(configuration ["Jwt:AccessTokenExpirationMinutes"])),
            }, AuthResults.Success);
        }
        public async Task<Result<RefreshResponse>> Refresh(TokenRequest request)
        {
            var principal = JWTGenerator.GetPrincipalFromExpiredToken(request.AccessToken, configuration);
            if (principal == null)
            {
                Result.Failure<RefreshResponse>(AuthResults.InvalidToken);
            }
            var email = principal!.FindFirst(ClaimTypes.Email)?.Value!;
            if (email.IsNullOrEmpty())
            {
                return Result.Failure<RefreshResponse>(AuthResults.InvalidToken);
            }
            var user = await userLoginDataRepository.GetFullUserDataByEmail(email);
            if (user is null)
            {
                return Result.Failure<RefreshResponse>(AuthResults.UserNotFound);
            };
            if (user.RefreshToken is null ||
                user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpTime < DateTime.Now)
            {
                return Result.Failure<RefreshResponse>(AuthResults.InvalidToken);
            }

            var newAccessToken = JWTGenerator.GenerateAccessToken(user.ID, user.UserAccountID, email, configuration);
            var newRefreshToken = JWTGenerator.GenerateAndHashSecureToken();

            var refreshTokenExpirationTime = DateTime.Now.AddDays(Convert.ToDouble(configuration ["Jwt:RefreshTokenExpirationDays"]));
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpTime = refreshTokenExpirationTime;
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

                return Result.Failure<UserAccountDTO>(AuthResults.NotAuthenticated);
            }
            var userLoginData = await userLoginDataRepository.GetByUserAccountID(AuthUser.ID);

            if (userLoginData is null)
            {
                return Result.Failure(AuthResults.UserNotFound);
            }
            userLoginData.RefreshToken = null;
            userLoginData.RefreshTokenExpTime = null;

            await userLoginDataRepository.Update(userLoginData);
            await cacheService.RemoveAsync(CacheUtils.AuthorizationCacheKey(AuthUser.ID));

            return Result.Success(AuthResults.Logouted);
        }
        public async Task<Result<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            var userLoginData = await userLoginDataRepository.GetByEmail(request.Email);
            if (userLoginData is null)
            {
                return Result.Success<string>(string.Empty, AuthResults.CheckEmail);
            }
            var recoveryToken = JWTGenerator.GenerateAndHashSecureToken();
            var recoveryTokenTime = DateTime.Now.AddMinutes(Convert.ToDouble(configuration ["Jwt:RecoveryTokenExpirationMinutes"]));
            userLoginData.RecoveryToken = recoveryToken;
            userLoginData.RecoveryTokenExpTime = recoveryTokenTime;
            await userLoginDataRepository.Update(userLoginData);

            // TODO instead of returning recovery token need to send email to user
            return Result.Success(recoveryToken, AuthResults.CheckEmail);
        }
        public async Task<Result> ResetPassword(ResetPasswordRequest request)
        {
            var userLoginData = await userLoginDataRepository.GetByRecoveryToken(request.RecoveryToken);

            if (userLoginData is null)
            {
                return Result.Failure(AuthResults.InvalidToken);
            }
            if (userLoginData.RecoveryTokenExpTime < DateTime.Now)
            {
                return Result.Failure(AuthResults.TokenExpired);
            }

            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);
            userLoginData!.PasswordHash = hash;
            userLoginData.PasswordSalt = salt;
            userLoginData.RecoveryToken = null;
            userLoginData.RecoveryTokenExpTime = null;

            await userLoginDataRepository.Update(userLoginData);

            return Result.Success(AuthResults.PasswordReseted);
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

                return Result.Failure<UserAccountDTO>(AuthResults.NotAuthenticated);
            }
        }
        public async Task<Result> VerifyEmail(string token)
        {
            var userLoginData = await userLoginDataRepository.GetByVerificationToken(token);
            if (userLoginData is null)
            {
                return Result.Failure(AuthResults.UserNotFound);
            }
            if (userLoginData.VerificationTokenExpTime < DateTime.Now)
            {
                return Result.Failure(AuthResults.TokenExpired);
            }

            userLoginData.VerificationToken = null;
            userLoginData.VerificationTokenExpTime = null;
            userLoginData.VerificationStatus = VerificationStatus.Verified;
            await userLoginDataRepository.Update(userLoginData);

            return Result.Success(AuthResults.EmailVerified);
        }
        public async Task<Result<RegisterResponse>> ChangeEmail(ChangeEmailRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();
            if (await userLoginDataRepository.GetByEmail(request.Email) != null)
            {
                return Result.Failure<RegisterResponse>(AuthResults.EmailAlreadyExists);
            }
            var userLoginData = await userLoginDataRepository.GetByUserAccountID(AuthUser.ID);
            if (userLoginData is null)
            {
                return Result.Failure<RegisterResponse>(AuthResults.UserNotFound);
            }
            if (userLoginData.VerificationStatus != VerificationStatus.Verified)
            {
                return Result.Failure<RegisterResponse>(AuthResults.EmailNotVerified);
            }
            if (userLoginData.VerificationTokenExpTime != null && userLoginData.VerificationTokenExpTime > DateTime.Now)
            {
                return Result.Failure<RegisterResponse>(AuthResults.EmailChangeAlreadyRequested);
            }
            var verificationToken = JWTGenerator.GenerateAndHashSecureToken();
            var expDays = Convert.ToDouble(configuration ["Jwt:VerificationTokenExpirationDays"]);
            var verificationTokenExpirationTime = DateTime.Now.AddDays(expDays);

            userLoginData.VerificationToken = verificationToken;
            userLoginData.VerificationTokenExpTime = verificationTokenExpirationTime;

            await userLoginDataRepository.Update(userLoginData);

            return Result.Success(new RegisterResponse()
            {
                VerificationToken = verificationToken,
                VerificationTokenExpTime = verificationTokenExpirationTime
            }, AuthResults.CheckEmail);
        }

        public async Task<Result> ChangePassword(ChangePasswordRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();
            var userLoginData = await userLoginDataRepository.GetByUserAccountID(AuthUser.ID);
            if (!PasswordHasher.VerifyPassword(request.CurrentPassword, userLoginData!.PasswordHash, userLoginData.PasswordSalt))
            {
                return Result.Failure(AuthResults.InvalidPassword);
            }
            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);
            userLoginData.PasswordHash = hash;
            userLoginData.PasswordSalt = salt;
            await userLoginDataRepository.Update(userLoginData);

            return Result.Success(AuthResults.PasswordChanged);
        }

        public async Task<Result> Update(UpdateUserRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();
            var userAccount = await userAccountRepository.Get(AuthUser.ID);
            userAccount!.FirstName = request.FirstName;
            userAccount!.LastName = request.LastName;
            userAccount.DateOfBirth = request.DateOfBirth;
            userAccount.Gender = request.Gender;
            await userAccountRepository.Update(userAccount);

            return Result.Success(AuthResults.UserUpdated);
        }
    }
}
