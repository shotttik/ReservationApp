namespace Application.Common.Results
{
    public class NotificationInboxResults
    {
        #region Errors
        public static readonly Error NotFound = Error.Validation("NotificationInbox.NotFound", "Notification not found.");
        #endregion
        #region Success
        public static readonly SuccessInfo Deleted = new("NotificationInbox.Deleted", "Notification deleted successfully.");
        public static readonly SuccessInfo DeletedAll = new("NotificationInbox.DeletedAll", "All notifications deleted successfully.");
        public static readonly SuccessInfo Read = new("NotificationInbox.Read", "Notification marked as read.");
        public static readonly SuccessInfo ReadAll = new("NotificationInbox.ReadAll", "All notifications marked as read.");
        #endregion

    }
}
