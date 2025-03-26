using API.Attributes;
using Application.Common.ResultsErrors;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController :ControllerBase
    {
        private readonly IUserService userAccountService;

        public UserController(IUserService userAccountService)
        {
            this.userAccountService = userAccountService;
        }

        [HttpPost("register")]
        [Logging(LoggingType.ExceptBody)]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerUserRequest)
        {
            Result result = await userAccountService.Register(registerUserRequest);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }

        [HttpPost("login")]
        [Logging(LoggingType.ExceptBody)]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            Result<LoginResponse> result = await userAccountService.Login(loginRequest);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
        }

        [HttpPost("refresh-token")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest refreshTokenRequest)
        {
            Result<RefreshResponse> result = await userAccountService.Refresh(refreshTokenRequest);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
        }

        [HttpPost("logout")]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> Logout([FromBody] TokenRequest logoutRequest)
        {
            var result = await userAccountService.Logout(logoutRequest);

            return result.IsSuccess ? Ok() : result.ToProblemDetails();
        }

        [HttpPost("forgot-password")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            Result<string> result = await userAccountService.ForgotPassword(forgotPasswordRequest);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
        }

        [HttpPost("reset-password")]
        [Logging(LoggingType.ExceptBody)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            Result result = await userAccountService.ResetPassword(resetPasswordRequest);

            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }

        [HttpGet("authorization-data")]
        [Authorize]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> GetUserAuthorizationDataAsync()
        {
            Result<UserAccountDTO> result = await userAccountService.GetUserAuthorizationDataAsync();

            return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
        }
        [HttpGet("verify-email")]
        [Logging(LoggingType.Full)]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            Result result = await userAccountService.VerifyEmail(token);
            return result.IsSuccess ? Ok(result) : result.ToProblemDetails();
        }
    }
}
