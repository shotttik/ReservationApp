using Application.Common.ResultsErrors;
using Application.DTOs.User;
using Application.Helpers;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Application.Services
{
    public class UserService :IUserService
    {
        private readonly IConfiguration configuration;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IUserLoginDataRepository userLoginDataRepository;

        public UserService(
            IConfiguration configuration,
            IUserAccountRepository userAccountRepository,
            IUserLoginDataRepository userLoginDataRepository)
        {
            this.configuration = configuration;
            this.userAccountRepository = userAccountRepository;
            this.userLoginDataRepository = userLoginDataRepository;
        }

        public async Task RegisterRequest(RegisterUserRequest request)
        {
            (byte [] hash, byte [] salt) = PasswordHasher.HashPassword(request.Password);

            var userAccount = new UserAccount
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
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
        }
        public async Task<Result<LoginResponse>> Login(LoginRequest request)
        {
            var userLoginData = await userLoginDataRepository.GetByEmailAsync(request.Email);

            if (userLoginData == null)
            {
                return Result.Failure<LoginResponse>(LoginErrors.NotFound);
            }
            if (!PasswordHasher.VerifyPassword(request.Password, userLoginData.PasswordHash, userLoginData.PasswordSalt))
            {
                return Result.Failure<LoginResponse>(LoginErrors.InvalidPassword);
            }

            var accessToken = JWTGenerator.GenerateAccessToken(userLoginData.Email, configuration);
            var refreshToken = JWTGenerator.GenerateRefreshToken();

            var refreshTokenExpirationTime = DateTime.Now.AddDays(Convert.ToDouble(configuration ["Jwt:RefreshTokenExpirationDays"]));
            await userLoginDataRepository.UpdateRefreshToken(userLoginData.ID, refreshToken, refreshTokenExpirationTime);

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
            //if (principal == null)
            //return BadRequest("Invalid access token or refresh token");

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var userLoginData = await userLoginDataRepository.GetByEmailAsync(email);
            if (userLoginData is null)
            {
                return Result.Failure<RefreshResponse>(RefreshTokenErrors.NotFound);
            };
            if (userLoginData.RefreshToken is null ||
                userLoginData.RefreshToken != request.RefreshToken ||
                userLoginData.RefreshTokenExpirationTime < DateTime.Now)
            {
                return Result.Failure<RefreshResponse>(RefreshTokenErrors.InvalidToken);
            }

            var newAccessToken = JWTGenerator.GenerateAccessToken(email, configuration);
            var newRefreshToken = JWTGenerator.GenerateRefreshToken();

            var refreshTokenExpirationTime = DateTime.Now.AddDays(Convert.ToDouble(configuration ["Jwt:RefreshTokenExpirationDays"]));
            await userLoginDataRepository.UpdateRefreshToken(userLoginData.ID, newRefreshToken, refreshTokenExpirationTime);

            var response = new RefreshResponse()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
            return response;

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
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
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
    }
}
