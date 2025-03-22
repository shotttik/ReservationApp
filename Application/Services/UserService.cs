using Application.Authentication;
using Application.Common.ResultsErrors;
using Application.Common.ResultsErrors.User;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace Application.Services
{
    public class UserService :IUserService
    {
        private readonly IConfiguration configuration;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IUserLoginDataRepository userLoginDataRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IDistributedCache cache;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);

        public UserService(
            IConfiguration configuration,
            IUserAccountRepository userAccountRepository,
            IUserLoginDataRepository userLoginDataRepository,
            IRoleRepository roleRepository,
            IDistributedCache cache,
            IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            this.userAccountRepository = userAccountRepository;
            this.userLoginDataRepository = userLoginDataRepository;
            this.roleRepository = roleRepository;
            this.cache = cache;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> RegisterRequest(RegisterUserRequest request)
        {
            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);
            if (await userLoginDataRepository.GetByEmailAsync(request.Email) != null)
            {
                return Result.Failure(RegisterErrors.AlreadyExists);
            }
            var role = await roleRepository.GetRole(request.RoleID);
            if (role is null)
            {
                return Result.Failure(RegisterErrors.RoleNotFound);
            }
            if (!(role.ID == Role.User.ID || role.ID == Role.Company.ID))
            {
                return Result.Failure(RegisterErrors.RoleIsNotAccessable);
            }
            var userAccount = new UserAccount
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
                RoleID = role.ID,
                Role = role
            };

            var userLoginData = new UserLoginData
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
        public async Task<Result<LoginResponse>> Login(LoginRequest request)
        {
            var user = await userLoginDataRepository.GetFullUserDataByEmailAsync(request.Email);

            if (user == null)
            {
                return Result.Failure<LoginResponse>(LoginErrors.NotFound);
            }
            if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Result.Failure<LoginResponse>(LoginErrors.InvalidPassword);
            }
            var accessToken = JWTGenerator.GenerateAccessToken(user.ID, user.Email, configuration);
            var refreshToken = JWTGenerator.GenerateAndHashSecureToken();

            var refreshTokenExpirationTime = DateTime.Now.AddDays(Convert.ToDouble(configuration ["Jwt:RefreshTokenExpirationDays"]));
            await userLoginDataRepository.UpdateRefreshToken(user.ID, refreshToken, refreshTokenExpirationTime);

            return Result.Success(new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpirationTime = DateTime.Now.AddMinutes(Convert.ToDouble(configuration ["Jwt:AccessTokenExpirationMinutes"])),
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
            var user = await userLoginDataRepository.GetFullUserDataByEmailAsync(email);
            if (user is null)
            {
                return Result.Failure<RefreshResponse>(RefreshTokenErrors.NotFound);
            };
            if (user.RefreshToken is null ||
                user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpirationTime < DateTime.Now)
            {
                return Result.Failure<RefreshResponse>(RefreshTokenErrors.InvalidToken);
            }

            var newAccessToken = JWTGenerator.GenerateAccessToken(user.ID, email, configuration);
            var newRefreshToken = JWTGenerator.GenerateAndHashSecureToken();

            var refreshTokenExpirationTime = DateTime.Now.AddDays(Convert.ToDouble(configuration ["Jwt:RefreshTokenExpirationDays"]));
            await userLoginDataRepository.UpdateRefreshToken(user.ID, newRefreshToken, refreshTokenExpirationTime);

            var response = new RefreshResponse()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
            return Result.Success(response);

        }
        public async Task<Result> Logout(TokenRequest request)
        {
            var refreshToken = request.RefreshToken;
            var accessToken = request.AccessToken;
            if (refreshToken is null || accessToken is null)
            {
                return Result.Failure(LogoutErrors.SameUser);
            }
            var principal = JWTGenerator.GetPrincipalFromExpiredToken(request.AccessToken, configuration);
            var email = principal.FindFirst(ClaimTypes.Email)?.Value!;

            var userLoginData = await userLoginDataRepository.GetByEmailAsync(email);

            if (userLoginData is null)
            {
                return Result.Failure(LogoutErrors.NotFound);
            }
            if (userLoginData.RefreshToken is null ||
                userLoginData.RefreshToken != request.RefreshToken ||
                userLoginData.RefreshTokenExpirationTime < DateTime.Now)
            {
                return Result.Failure(LogoutErrors.InvalidToken); // should result patterni
            }
            await userLoginDataRepository.UpdateRefreshToken(userLoginData.ID, null, DateTime.Now);

            return Result.Success();
        }
        public async Task<Result<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            var userLoginData = await userLoginDataRepository.GetByEmailAsync(request.Email);
            if (userLoginData is null)
            {
                return Result.Failure<string>(ForgotPasswordErrors.NotFound);
            }
            var recoveryToken = JWTGenerator.GenerateAndHashSecureToken();
            var recoveryTokenTime = DateTime.Now.AddMinutes(Convert.ToDouble(configuration ["Jwt:RecoveryTokenExpirationMinutes"]));
            await userLoginDataRepository.UpdateRecoveryToken(userLoginData.ID, recoveryToken, recoveryTokenTime);

            // TODO instead of returning recovery token need to send email to user
            return Result.Success(recoveryToken);
        }
        public async Task<Result> ResetPassword(ResetPasswordRequest request)
        {
            var userLoginData = await userLoginDataRepository.GetByEmailAsync(request.Email);

            if (userLoginData is null)
            {
                return Result.Failure(ResetPasswordErrors.NotFound);
            }
            if (userLoginData.PasswordRecoveryToken is null ||
                userLoginData.PasswordRecoveryToken != request.RecoveryToken ||
                userLoginData.RecoveryTokenTime < DateTime.Now)
            {
                return Result.Failure(ResetPasswordErrors.InvalidToken);
            }

            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);
            await userLoginDataRepository.UpdateResetPasswordData(userLoginData.ID, hash, salt);

            return Result.Success();
        }
        public async Task<Result<UserAccountDTO>> GetUserAuthorizationDataAsync()
        {
            var userEmail = httpContextAccessor.HttpContext?.User.Claims
                   .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (userEmail == null)
                return Result.Failure<UserAccountDTO>(AuthorizationDataErrors.NotFound);
            var userLoginData = await userLoginDataRepository.GetByEmailAsync(userEmail);
            if (userLoginData == null)
                return Result.Failure<UserAccountDTO>(AuthorizationDataErrors.NotFound);
            var userID = userLoginData.UserAccountID;
            var cacheKey = $"UserAuthorization:{userID}";
            var cachedData = await cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return Result.Success(JsonSerializer.Deserialize<UserAccountDTO>(cachedData)!);
            }
            var user = await userAccountRepository.GetAuthorizationData(userID);

            if (user == null)
                return Result.Failure<UserAccountDTO>(AuthorizationDataErrors.NotFound);

            var userDTO = new UserAccountDTO
            {
                ID = user.ID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Role = new RoleDTO
                {
                    ID = user.Role.ID,
                    Name = user.Role.Name,
                    Permissions = user.Role.Permissions.Select(p => new PermissionDTO
                    {
                        ID = p.ID,
                        Name = p.Name
                    }).ToList()
                }
            };

            var serializedData = JsonSerializer.Serialize(userDTO);
            await cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheExpiration
            });

            return Result.Success(userDTO);
        }
    }
}
