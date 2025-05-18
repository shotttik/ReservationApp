using Domain.Abstractions;
using System.Linq.Dynamic.Core;

namespace Domain.Extensions
{
    public static class QueryableExtensions
    {
        private static readonly string [] SupportedOperators = new [] { "==", "!=", ">=", "<=", ">", "<", "~=" };

        public static IQueryable<T> ApplyQueryParamsAsync<T>(
            this IQueryable<T> query,
            PagedParameters queryParams)
        {
            var allowedProps = typeof(T).GetProperties()
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Filtering
            if (!string.IsNullOrWhiteSpace(queryParams.Filter))
            {
                var filters = queryParams.Filter.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var raw in filters)
                {
                    var op = SupportedOperators.FirstOrDefault(o => raw.Contains(o));
                    if (op == null) continue;

                    var parts = raw.Split(op, 2);
                    if (parts.Length != 2) continue;

                    var field = parts [0].Trim();
                    var value = parts [1].Trim();

                    if (!allowedProps.Contains(field)) continue;

                    var expression = op switch
                    {
                        "==" => $"{field} == @0",
                        "!=" => $"{field} != @0",
                        ">" => $"{field} > @0",
                        ">=" => $"{field} >= @0",
                        "<" => $"{field} < @0",
                        "<=" => $"{field} <= @0",
                        "~=" => $"{field}.Contains(@0)",
                        _ => null
                    };
                   
                    if (expression != null)
                    {
                        query = query.Where(expression, value);
                    }
                }
            }
            // Sorting
            if (!string.IsNullOrWhiteSpace(queryParams.SortBy))
            {
                var clauses = queryParams.SortBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var sortParts = new List<string>();

                foreach (var clause in clauses)
                {
                    var parts = clause.Split(':');
                    var field = parts [0].Trim();
                    var dir = parts.Length == 2 && parts [1].Trim().ToLower() == "desc" ? "desc" : "asc";

                    if (allowedProps.Contains(field))
                        sortParts.Add($"{field} {dir}");
                }

                if (sortParts.Count > 0)
                    query = query.OrderBy(string.Join(", ", sortParts));
            }
            return query;
        }
    }
}
