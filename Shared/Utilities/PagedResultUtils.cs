namespace Shared.Utilities
{
    public static class PagedResultUtils
    {
        public static int DefaultPageNumber = 1;
        public static int DefaultPageSize = 10;
        public static int MaxPageSize = 100;
        public static int MinPageSize = 1;
        // operators
        public static string [] Operators = ["==", "!=", ">=", "<=", ">", "<", "~="];
    }
}
