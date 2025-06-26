using API.Attributes;
using Application.Common.Requests.User;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users/me")]
    [ApiController]
    [Tags("Users")]
    public class UserController :ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Gets the authorization data for the current logged-in user.
        /// </summary>
        /// <returns>Basic profile and role info of the current user.</returns>
        [HttpGet("profile")]
        [Authorize]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse<AuthUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserAuthorizationData()
        {
            var result = await userService.GetUserAuthorizationData();

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
        [ProducesResponseType(typeof(SuccessResponse<RegisterResponse>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var result = await userService.Update(request);
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
