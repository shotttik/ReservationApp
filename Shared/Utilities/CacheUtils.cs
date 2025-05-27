namespace Shared.Utilities
{
    public static class CacheUtils
    {
        public static string SessionKey(string sessionID) => $"Session:{sessionID}";
        public static string ActiveSessionsKey(int userID) => $"UserSessions:{userID}";
    }
}
