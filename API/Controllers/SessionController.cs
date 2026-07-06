using API.Attributes;
using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users/me/sessions")]
    [ApiController]
    [Tags("User Sessions")]
    public class SessionController :ControllerBase
    {
        private readonly IUserService userService;

        public SessionController(IUserService userService)
        {
            this.userService = userService;
        }
        /// <summary>
        /// Retrieves all active sessions for the current user.
        /// </summary>
        /// <returns>List of active sessions including current one.</returns>
        [HttpGet]
        [Logging(LoggingType.Full)]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<List<SessionInfoSummaryDTO>>), StatusCodes.Status200OK)]
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
        [HttpDelete("{sessionId}")]
        [Logging(LoggingType.Full)]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteActiveSession(string sessionId)
        {
            var result = await userService.DeleteActiveSession(sessionId);

            return result.ToResponse();
        }
        /// <summary>
        /// Deletes active sessions for the current user except current one.
        /// </summary>
        /// <returns>Success if all sessions were removed.</returns>
        [HttpDelete]
        [Logging(LoggingType.Full)]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAllActiveSessionsExceptCurrent()
        {
            var result = await userService.DeleteAllActiveSessions(ExceptCurrent: true);
            return result.ToResponse();
        }
    }
}
