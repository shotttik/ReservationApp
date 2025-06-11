using API.Attributes;
using Application.Common.Requests.User;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    [Tags("Authentication & Authorization")]
    public class AuthController :ControllerBase
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }
        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="registerUserRequest">User registration details including email, password, and profile info.</param>
        /// <returns>Success with a verification token or error if registration fails.</returns>
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
        /// <summary>
        /// Authenticates a user and returns an access/refresh token pair.
        /// </summary>
        /// <param name="loginRequest">Login credentials (email and password).</param>
        /// <returns>Access and refresh tokens if successful.</returns>
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
        /// <summary>
        /// Refreshes a JWT using a valid refresh token.
        /// </summary>
        /// <param name="refreshTokenRequest">Request containing expired access token and valid refresh token.</param>
        /// <returns>New access and refresh tokens.</returns>
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
        /// <summary>
        /// Logs the current user out and invalidates their session.
        /// </summary>
        /// <returns>Success if session was removed.</returns>
        [HttpPost("logout")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout()
        {
            var result = await userService.Logout();

            return result.ToResponse();
        }
        /// <summary>
        /// Initiates password recovery by sending a recovery token to the user's email.
        /// </summary>
        /// <param name="forgotPasswordRequest">User email to send the token to.</param>
        /// <returns>Token (for dev/testing) or email notification trigger.</returns>
        [HttpPost("forgot-password")]
        [Logging(LoggingType.ExceptBody)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            var result = await userService.ForgotPassword(forgotPasswordRequest);

            return result.ToResponse();
        }
        /// <summary>
        /// Resets the user's password using a valid recovery token.
        /// </summary>
        /// <param name="resetPasswordRequest">New password and recovery token.</param>
        /// <returns>Success if password was changed.</returns>
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
        /// <summary>
        /// Verifies a user's email using a token from the registration or email change process.
        /// </summary>
        /// <param name="token">Email verification token.</param>
        /// <returns>Success if email is verified.</returns>
        [HttpGet("verify-email")]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var result = await userService.VerifyEmail(token);
            return result.ToResponse();
        }
    }
}
