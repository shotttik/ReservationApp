namespace Shared.Utilities
{
    public static class RedisUtils
    {
        public static string AuthorizationCacheKey(int userID) => $"UserAuthorization:{userID}";
    }
}
