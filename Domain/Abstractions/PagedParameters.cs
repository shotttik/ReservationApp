namespace Domain.Abstractions
{
    public class PagedParameters
    {
        public string? Filter { get; set; }
        public string? SortBy { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public IEnumerable<string> Validate<TDto>()
        {
            var errors = new List<string>();
            var props = typeof(TDto).GetProperties()
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (PageNumber <= 0)
                errors.Add("Page must be greater than 0.");
            if (PageSize is < 1 or > 100)
                errors.Add("PageSize must be between 1 and 100.");

            if (!string.IsNullOrWhiteSpace(SortBy))
            {
                var fields = SortBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var field in fields)
                {
                    var parts = field.Split(':');
                    var column = parts [0].Trim();
                    var dir = parts.Length == 2 ? parts [1].Trim().ToLower() : "asc";

                    if (!props.Contains(column))
                        errors.Add($"Sort field '{column}' not valid.");
                    if (dir is not "asc" and not "desc")
                        errors.Add($"Sort direction '{dir}' not valid.");
                }
            }

            var operators = new [] { "==", "!=", ">=", "<=", ">", "<", "~=" };

            if (!string.IsNullOrWhiteSpace(Filter))
            {
                var filters = Filter.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var raw in filters)
                {
                    var op = operators.FirstOrDefault(o => raw.Contains(o));
                    if (op == null)
                    {
                        errors.Add($"Invalid filter operator in: {raw}");
                        continue;
                    }

                    var parts = raw.Split(op, 2);
                    if (parts.Length != 2)
                    {
                        errors.Add($"Malformed filter: {raw}");
                        continue;
                    }

                    var field = parts [0].Trim();
                    if (!props.Contains(field))
                        errors.Add($"Filter field '{field}' not valid.");
                }
            }

            return errors;
        }

    }
}
