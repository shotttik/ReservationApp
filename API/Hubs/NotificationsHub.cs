using Domain.DTO;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Utilities;
using System.Security.Claims;

namespace API.Hubs
{
    /// <summary>
    /// SignalR hub used by frontend clients to receive live notification events.
    /// </summary>
    /// <remarks>
    /// Vue should connect to <c>/hubs/notifications</c> with the same JWT access token used for API requests.  
    /// The frontend should listen for event name <c>notification</c>.  
    /// This hub handles live delivery only; missed notifications should be loaded from
    /// <c>GET /api/v1/notifications/mine</c>.
    /// </remarks>
    [Authorize(AuthenticationSchemes = "Bearer,Guest")]
    public class NotificationsHub :Hub
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<NotificationsHub> _logger;

        public NotificationsHub(
            ICacheService cacheService,
            ILogger<NotificationsHub> logger)
        {
            _cacheService = cacheService;
            _logger = logger;
        }

        /// <summary>
        /// Adds the connected client to user, company, branch, or guest booking notification groups.
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            await JoinUserGroupsAsync();
            await JoinGuestGroupsAsync();
            await base.OnConnectedAsync();
        }

        private async Task JoinUserGroupsAsync()
        {
            var user = Context.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                return;
            }

            var userAccountId = user.FindFirst(ClaimTypes.Sid)?.Value;
            if (!string.IsNullOrWhiteSpace(userAccountId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, NotificationGroups.User(userAccountId));
            }

            var sessionId = user.FindFirst("SessionID")?.Value;
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return;
            }

            var session = await _cacheService.GetAsync<SessionInfoDTO>(CacheUtils.SessionKey(sessionId));
            if (session?.AuthUser == null)
            {
                _logger.LogWarning("SignalR connection {ConnectionId} has no cached session data.", Context.ConnectionId);
                return;
            }

            if (session.AuthUser.CompanyId.HasValue)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, NotificationGroups.Company(session.AuthUser.CompanyId.Value));
            }

            if (session.AuthUser.BranchId.HasValue)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, NotificationGroups.Branch(session.AuthUser.BranchId.Value));
            }
        }

        private async Task JoinGuestGroupsAsync()
        {
            var bookingId = Context.User?.FindFirst("bookingId")?.Value;
            var scope = Context.User?.FindFirst("scope")?.Value;

            if (scope == "booking:guest" && !string.IsNullOrWhiteSpace(bookingId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, NotificationGroups.GuestBooking(bookingId));
            }
        }
    }

    public static class NotificationGroups
    {
        public static string User(int userAccountId) => User(userAccountId.ToString());
        public static string User(string userAccountId) => $"user:{userAccountId}";
        public static string Company(int companyId) => $"company:{companyId}";
        public static string Branch(int branchId) => $"branch:{branchId}";
        public static string GuestBooking(int bookingId) => GuestBooking(bookingId.ToString());
        public static string GuestBooking(string bookingId) => $"guest-booking:{bookingId}";
    }
}
