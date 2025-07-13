using Domain.Abstractions;
using Shared.Utilities;

namespace Application.Extensions.Mappers.Pagination
{
    public static class PagedParameterMapper
    {
        public static PagedParameters MapToEntityPaths(PagedParameters input, IDictionary<string, string> fieldMap)
        {
            return new PagedParameters
            {
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                Filter = MapFilter(input.Filter, fieldMap),
                SortBy = MapSort(input.SortBy, fieldMap)
            };
        }

        private static string? MapFilter(string? filter, IDictionary<string, string> fieldMap)
        {
            if (string.IsNullOrWhiteSpace(filter)) return filter;

            return string.Join(",", filter
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(condition =>
                {
                    var op = PagedResultUtils.Operators.FirstOrDefault(o => condition.Contains(o));
                    if (op == null) return condition;

                    var idx = condition.IndexOf(op);
                    var field = condition [..idx].Trim();
                    var value = condition [(idx + op.Length)..];

                    return fieldMap.TryGetValue(field, out var mappedField)
                        ? $"{mappedField}{op}{value}"
                        : condition;
                }));
        }

        private static string? MapSort(string? sortBy, IDictionary<string, string> fieldMap)
        {
            if (string.IsNullOrWhiteSpace(sortBy)) return sortBy;

            return string.Join(",", sortBy
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(entry =>
                {
                    var parts = entry.Split(':', 2);
                    var field = parts [0].Trim();
                    var direction = parts.Length == 2 ? $":{parts [1].Trim()}" : "";

                    return fieldMap.TryGetValue(field, out var mappedField)
                        ? $"{mappedField}{direction}"
                        : entry;
                }));
        }
    }
}
