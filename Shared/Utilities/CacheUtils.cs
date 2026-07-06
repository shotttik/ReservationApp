namespace Shared.Utilities
{
    public static class CacheUtils
    {
        public static string SessionKey(string sessionID) => $"Session:{sessionID}";
        public static string UserSessionsKey(int userID) => $"UserSessions:{userID}";
    }
}
