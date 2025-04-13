namespace Shared.Utilities
{
    public static class CacheUtils
    {
        public static string AuthorizationCacheKey(int userID) => $"UserAuthorization:{userID}";
    }
}
