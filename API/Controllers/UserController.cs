using API.Attributes;
using Application.Common.Requests.User;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO;
using Domain.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController :ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
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
            var result = await userService.Register(registerUserRequest);

            return result.ToResponse();
        }

        [HttpPost("login")]
        [Logging(LoggingType.ExceptBody)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await userService.Login(loginRequest);

            return result.ToResponse();
        }

        [HttpPost("refresh-token")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<RefreshResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var result = await userService.Refresh(refreshTokenRequest);

            return result.ToResponse();
        }

        [HttpPost("logout")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout()
        {
            var result = await userService.Logout();

            return result.ToResponse();
        }

        [HttpPost("forgot-password")]
        [Logging(LoggingType.Full)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            var result = await userService.ForgotPassword(forgotPasswordRequest);

            return result.ToResponse();
        }

        [HttpPost("reset-password")]
        [Logging(LoggingType.ExceptBody)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            var result = await userService.ResetPassword(resetPasswordRequest);

            return result.ToResponse();
        }

        [HttpGet("authorization-data")]
        [Authorize]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<UserAccountDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserAuthorizationDataAsync()
        {
            var result = await userService.GetUserAuthorizationDataAsync();

            return result.ToResponse();
        }
        [HttpGet("verify-email")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var result = await userService.VerifyEmail(token);
            return result.ToResponse();
        }
        [HttpPost("change-email")]
        [Logging(LoggingType.Full)]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<Result<RegisterResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequest request)
        {
            var result = await userService.ChangeEmail(request);

            return result.ToResponse();
        }
        [HttpPost("change-password")]
        [Logging(LoggingType.Full)]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await userService.ChangePassword(request);

            return result.ToResponse();
        }
        [HttpPut]
        [Logging(LoggingType.Full)]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var result = await userService.Update(request);
            return result.ToResponse();
        }

        [HttpGet("sessions")]
        [Logging(LoggingType.Full)]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<Result<List<SessionInfoSummaryDTO>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveSessions()
        {
            var result = await userService.GetActiveSessions();

            return result.ToResponse();
        }
    }
}
