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
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var userLoginData = await userLoginDataRepository.GetByEmailAsync(request.Email);

            if (userLoginData == null)
            {
                throw new Exception("Invalid Email"); // will be handled by middleware result pattern
            }
            if (!PasswordHasher.VerifyPassword(request.Password, userLoginData.PasswordHash, userLoginData.PasswordSalt))
            {
                throw new Exception("Invalid email or password"); // will be handled by middleware result pattern
            }

            var accessToken = JWTGenerator.GenerateAccessToken(userLoginData.Email, configuration);
            var refreshToken = JWTGenerator.GenerateRefreshToken();

            var refreshTokenExpirationTime = DateTime.Now.AddDays(Convert.ToDouble(configuration ["Jwt:RefreshTokenExpirationDays"]));
            await userLoginDataRepository.UpdateRefreshToken(userLoginData.ID, refreshToken, refreshTokenExpirationTime);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpirationTime = DateTime.Now.AddMinutes(Convert.ToDouble(configuration ["Jwt:AccessTokenExpirationMinutes"])),
            };
        }
        public async Task<RefreshResponse> Refresh(RefreshTokenRequest request)
        {
            var principal = JWTGenerator.GetPrincipalFromExpiredToken(request.AccessToken, configuration);
            //if (principal == null)
            //return BadRequest("Invalid access token or refresh token");

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var userLoginData = await userLoginDataRepository.GetByEmailAsync(email);
            if (userLoginData is null)
            {
                throw new Exception("user not found");
            };
            if (userLoginData.RefreshToken is null ||
                userLoginData.RefreshToken != request.RefreshToken ||
                userLoginData.RefreshTokenExpirationTime < DateTime.Now)
            {
                throw new Exception("InvalidRefreshToken"); // should result patterni
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
    }
}
