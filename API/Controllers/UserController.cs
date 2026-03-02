using API.Attributes;
using Application.Common.Requests.User;
using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/profile")]
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
        [HttpGet()]
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
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
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
        /// Updates basic user profile data like name, birthdate, gender or Image.
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
        [HttpDelete()]
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

        /// <summary>
        /// Uploads profile image for the user.
        /// </summary>
        /// <remarks>
        /// This endpoint allows uploading profile image. 
        /// returned value is the ID of the uploaded media record.
        /// and can be used to reference the image in user profile.
        /// <para><b>Required Roles:</b> <strong>Accessible by everyone</strong></para>
        /// <para><b>Max File Size:</b> 1 MB (1,048,576 bytes)</para>
        /// <para><b>Allowed File Types:</b> image/jpeg, image/png</para>
        /// </remarks>
        /// <param name="request">The request containing the media file to upload.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A result returns address of the image or failure of the upload operation.</returns>
        [HttpPost("profile-image")]
        [Authorize]
        [Logging(LoggingType.General)]
        [EnableRateLimiting("fixed")]
        [ProducesResponseType(typeof(SuccessResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadMedia([FromForm] UploadUserProfileImageRequest request, CancellationToken cancellationToken)
        {
            var result = await userService.UploadProfileImage(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
