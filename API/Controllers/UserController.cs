using API.Attributes;
using Application.Common.Results;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Responses;
using Domain.DTO;
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
        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="registerUserRequest">The registration details</param>
        /// <returns>Registration result</returns>
        [HttpPost("register")]
        [Logging(LoggingType.ExceptBody)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<RegisterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerUserRequest)
        {
            var result = await userAccountService.Register(registerUserRequest);

            return result.ToResponse();
        }

        [HttpPost("login")]
        [Logging(LoggingType.ExceptBody)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            Result<LoginResponse> result = await userAccountService.Login(loginRequest);

            return result.ToResponse();
        }

        [HttpPost("refresh-token")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<RefreshResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest refreshTokenRequest)
        {
            Result<RefreshResponse> result = await userAccountService.Refresh(refreshTokenRequest);

            return result.ToResponse();
        }

        [HttpPost("logout")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout()
        {
            var result = await userAccountService.Logout();

            return result.ToResponse();
        }

        [HttpPost("forgot-password")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            Result<string> result = await userAccountService.ForgotPassword(forgotPasswordRequest);

            return result.ToResponse();
        }

        [HttpPost("reset-password")]
        [Logging(LoggingType.ExceptBody)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            Result result = await userAccountService.ResetPassword(resetPasswordRequest);

            return result.ToResponse();
        }

        [HttpGet("authorization-data")]
        [Authorize]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<UserAccountDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserAuthorizationDataAsync()
        {
            Result<UserAccountDTO> result = await userAccountService.GetUserAuthorizationDataAsync();

            return result.ToResponse();
        }
        [HttpGet("verify-email")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            Result result = await userAccountService.VerifyEmail(token);
            return result.ToResponse();
        }
    }
}
