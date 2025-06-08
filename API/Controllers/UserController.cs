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
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/user")]
    [ApiController]
    [Tags("User Account")]
    public class UserController :ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
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
        [Logging(LoggingType.Full)]
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
        /// Gets the authorization data for the current logged-in user.
        /// </summary>
        /// <returns>Basic profile and role info of the current user.</returns>
        [HttpGet("authorization-data")]
        [Authorize]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<AuthUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserAuthorizationDataAsync()
        {
            var result = await userService.GetUserAuthorizationDataAsync();

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
        /// <summary>
        /// Requests to change the user's email. Sends a verification token to the new email.
        /// </summary>
        /// <param name="request">New email address.</param>
        /// <returns>Success with a verification token (dev/testing) or email sent.</returns>
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
        /// <summary>
        /// Changes the user's current password.
        /// </summary>
        /// <param name="request">Current and new password.</param>
        /// <returns>Success if password is updated.</returns>
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
        /// <summary>
        /// Updates basic user profile data like name, birthdate, or gender.
        /// </summary>
        /// <param name="request">Updated profile information.</param>
        /// <returns>Success if update was saved.</returns>
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
        /// <summary>
        /// Retrieves all active sessions for the current user.
        /// </summary>
        /// <returns>List of active sessions including current one.</returns>
        [HttpGet("sessions")]
        [Logging(LoggingType.Full)]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<Result<List<SessionInfoSummaryDTO>>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveSessions()
        {
            var result = await userService.GetActiveSessions();

            return result.ToResponse();
        }
        /// <summary>
        /// Deletes a specific active session by ID.
        /// </summary>
        /// <param name="sessionId">ID of the session to terminate.</param>
        /// <returns>Success if the session was removed.</returns>
        [HttpDelete("session/{sessionId}")]
        [Logging(LoggingType.Full)]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteActiveSession(string sessionId)
        {
            var result = await userService.DeleteActiveSession(sessionId);

            return result.ToResponse();
        }
        /// <summary>
        /// Deletes all active sessions for the current user.
        /// </summary>
        /// <returns>Success if all sessions were removed.</returns>
        [HttpDelete("sessions")]
        [Logging(LoggingType.Full)]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<Result>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAllActiveSessions()
        {
            var result = await userService.DeleteAllActiveSessions();
            return result.ToResponse();
        }
        /// <summary>
        /// Soft deletes the current user account (deactivation).
        /// </summary>
        /// <returns>Success if user is marked as deleted.</returns>
        [HttpDelete("user")]
        [Authorize]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UserDelete()
        {
            var result = await userService.Delete(null, false);

            return result.ToResponse();
        }
    }
}
