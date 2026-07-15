using API.Attributes;
using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Handles authenticated user's notification inbox.
    /// </summary>
    /// <remarks>
    /// Frontend flow for Vue clients:<br/>
    /// <ol>
    /// <li>Connect to SignalR hub at <c>/hubs/notifications</c> using the access token.</li>
    /// <li>Listen for the <c>notification</c> event for live updates.</li>
    /// <li>Call <c>GET /api/v1/notifications/mine</c> after login, refresh, or reconnect to load missed notifications.</li>
    /// <li>Call <c>POST /api/v1/notifications/{id}/read</c> when the user opens or dismisses a notification.</li>
    /// </ol>
    /// SignalR is for live delivery. This controller is for durable notification history and unread state.
    /// </remarks>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/notifications")]
    [ApiController]
    [Tags("Notifications")]
    [Authorize]
    public class NotificationsController :ControllerBase
    {
        private readonly INotificationInboxService _notificationInboxService;

        public NotificationsController(INotificationInboxService notificationInboxService)
        {
            _notificationInboxService = notificationInboxService;
        }

        /// <summary>
        /// Retrieves notifications for the current authenticated user.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>Accessible only authorized.</strong><br/><br/>
        /// Returns durable notifications that belong to the current user, user's company, or user's branch.  
        /// Use this endpoint from Vue after page load, login, refresh, or SignalR reconnect to recover notifications missed while the client was offline.  
        /// Live notifications are received separately from SignalR event <c>notification</c> on hub <c>/hubs/notifications</c>.
        /// </remarks>
        /// <param name="unreadOnly">When true, returns only notifications that have not been marked as read.</param>
        /// <param name="take">Maximum number of notifications to return. The server clamps this value between 1 and 100.</param>
        /// <param name="cancellationToken">Request cancellation token.</param>
        /// <returns>List of notification records for the current authenticated user.</returns>
        [HttpGet("mine")]
        [Logging(LoggingType.Full)]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse<List<NotificationDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMine(
            [FromQuery] bool unreadOnly = false,
            [FromQuery] int take = 50,
            CancellationToken cancellationToken = default)
        {
            var result = await _notificationInboxService.GetMineAsync(unreadOnly, take, cancellationToken);

            return result.ToResponse();
        }

        /// <summary>
        /// Marks a notification as read for the current authenticated user.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>Accessible only authorized.</strong><br/><br/>
        /// Use this endpoint when Vue marks a notification as opened, dismissed, or seen in the notification menu.  
        /// The notification must belong to the current user, user's company, or user's branch.
        /// </remarks>
        /// <param name="id">Notification ID.</param>
        /// <param name="cancellationToken">Request cancellation token.</param>
        /// <returns>Success message or error result.</returns>
        [HttpPost("{id:int}/read")]
        [Logging(LoggingType.Full)]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkRead([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _notificationInboxService.MarkReadAsync(id, cancellationToken);

            return result.ToResponse();
        }

        /// <summary>
        /// Marks all notifications as read for the current authenticated user.
        /// </summary>
        /// <remarks>
        /// Required role: <strong>Accessible only authorized.</strong><br/><br/>
        /// Use this endpoint when Vue marks all notifications as opened, dismissed, or seen in the
        /// notification menu.
        /// </remarks>
        /// <param name="cancellationToken">Request cancellation token.</param>
        /// <returns>Success message or error result.</returns>
        [HttpPost("read-all")]
        [Authorize]
        [Logging(LoggingType.Full)]
        [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MarkReadAll(CancellationToken cancellationToken)
        {
            var result = await _notificationInboxService.MarkReadAllAsync(cancellationToken);
            return result.ToResponse();
        }
    }
}
