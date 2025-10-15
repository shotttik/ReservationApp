using Shared.Utilities;
using System.Reflection;

namespace Domain.Abstractions
{
    public class PagedParameters
    {
        public string? Filter { get; set; }
        public string? SortBy { get; set; }
        public int PageNumber { get; set; } = PagedResultUtils.DefaultPageNumber;
        public int PageSize { get; set; } = PagedResultUtils.DefaultPageSize;
        
        public IEnumerable<string> Validate(IDictionary<string, string> allowedFields, Type entityType)
        {
            var errors = new List<string>();

            ValidatePagination(errors);
            ValidateSorting(errors, allowedFields);
            ValidateFiltering(errors, allowedFields, entityType);

            return errors;
        }

        private void ValidatePagination(List<string> errors)
        {
            if (PageNumber <= 0)
                errors.Add("Page must be greater than 0.");

            if (PageSize < PagedResultUtils.MinPageSize || PageSize > PagedResultUtils.MaxPageSize)
                errors.Add($"PageSize must be between {PagedResultUtils.MinPageSize} and {PagedResultUtils.MaxPageSize}.");
        }

        private void ValidateSorting(List<string> errors, IDictionary<string, string> allowedFields)
        {
            if (string.IsNullOrWhiteSpace(SortBy)) return;

            var fields = SortBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var field in fields)
            {
                var parts = field.Split(':');
                var column = parts [0].Trim();
                var direction = parts.Length == 2 ? parts [1].Trim().ToLower() : "asc";

                if (!allowedFields.ContainsKey(column))
                    errors.Add($"Sort field '{column}' is not allowed.");

                if (direction is not "asc" and not "desc")
                    errors.Add($"Sort direction '{direction}' is not valid. Use 'asc' or 'desc'.");
            }
        }

        private void ValidateFiltering(List<string> errors, IDictionary<string, string> allowedFields, Type entityType)
        {
            if (string.IsNullOrWhiteSpace(Filter)) return;

            var filters = Filter.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var raw in filters)
            {
                var op = PagedResultUtils.Operators.FirstOrDefault(o => raw.Contains(o));
                if (op == null)
                {
                    errors.Add($"Invalid filter operator in: '{raw}'.");
                    continue;
                }

                var parts = raw.Split(op, 2);
                if (parts.Length != 2)
                {
                    errors.Add($"Malformed filter: '{raw}'. Expected format: 'field{op}value'");
                    continue;
                }

                var field = parts [0].Trim();
                var value = parts [1].Trim();

                if (!allowedFields.TryGetValue(field, out var mappedPath))
                {
                    errors.Add($"Filter field '{field}' is not allowed.");
                    continue;
                }

                var validationError = ValidateFilterValue(mappedPath, field, op, value, entityType);
                if (validationError != null)
                    errors.Add(validationError);
            }
        }

        private static string? ValidateFilterValue(string propertyPath, string originalField, string op, string value, Type rootType)
        {
            try
            {
                var propertyType = GetPropertyTypeFromPath(rootType, propertyPath);
                if (propertyType == null) return null;

                var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                var isNullable = Nullable.GetUnderlyingType(propertyType) != null;

                if (value.Equals("null", StringComparison.OrdinalIgnoreCase))
                    return isNullable ? null : $"Field '{originalField}' is not nullable.";

                if (op is ">" or ">=" or "<" or "<=" && !IsNumericOrDateTime(underlyingType))
                    return $"Operator '{op}' can only be used on numeric/date fields and not for '{originalField}'.";

                if (op == "~=" && underlyingType != typeof(string))
                    return $"Contains operator '~=' only works with string fields not like '{originalField}'.";

                if (underlyingType == typeof(int) && !int.TryParse(value, out _))
                    return $"Value '{value}' is not a valid integer for field '{originalField}'.";

                if (underlyingType == typeof(Guid) && !Guid.TryParse(value, out _))
                    return $"Value '{value}' is not a valid GUID for field '{originalField}'.";

                if (underlyingType == typeof(bool) && !bool.TryParse(value, out _))
                    return $"Value '{value}' is not a valid boolean for field '{originalField}'.";

                if (underlyingType == typeof(DateTime) && !DateTime.TryParse(value, out _))
                    return $"Value '{value}' is not a valid DateTime for field '{originalField}'.";

                return !IsValidValue(underlyingType, value)
                    ? $"Value '{value}' is not valid for field '{originalField}'. Expected type: {underlyingType.Name}."
                    : null;
            }
            catch
            {
                return $"Unable to validate value '{value}' for field '{originalField}'.";
            }
        }

        private static bool IsValidValue(Type type, string value)
        {
            return type switch
            {
                var t when t == typeof(int) => int.TryParse(value, out _),
                var t when t == typeof(Guid) => Guid.TryParse(value, out _),
                var t when t == typeof(bool) => bool.TryParse(value, out _),
                var t when t == typeof(DateTime) => DateTime.TryParse(value, out _),
                _ => true
            };
        }

        private static bool IsNumericOrDateTime(Type type)
        {
            return type == typeof(int) || type == typeof(long) || type == typeof(float) ||
                   type == typeof(double) || type == typeof(decimal) || type == typeof(DateTime);
        }

        private static Type? GetPropertyTypeFromPath(Type rootType, string propertyPath)
        {
            var type = rootType;
            foreach (var part in propertyPath.Split('.'))
            {
                var prop = type.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) return null;

                if (IsCollectionProperty(prop.PropertyType))
                {
                    var elementType = GetCollectionElementType(prop.PropertyType);
                    if (elementType == null)
                        return null;

                    type = elementType;
                }
                else
                {
                    type = prop.PropertyType;
                }
            }
            return type;
        }

        private static bool IsNavigationProperty(Type type)
        {
            if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || Nullable.GetUnderlyingType(type) != null)
                return false;

            return type.IsClass && !IsCollectionProperty(type);
        }

        private static bool IsCollectionProperty(Type type)
        {
            return type != typeof(string) &&
                   typeof(System.Collections.IEnumerable).IsAssignableFrom(type);
        }

        private static Type? GetCollectionElementType(Type collectionType)
        {
            if (collectionType.IsGenericType)
                return collectionType.GetGenericArguments().FirstOrDefault();

            return collectionType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                ?.GetGenericArguments().FirstOrDefault();
        }

        private static void GetPropertyPaths(Type type, string prefix, HashSet<string> paths, int maxDepth, HashSet<Type> visited)
        {
            if (maxDepth <= 0 || visited.Contains(type)) return;

            visited.Add(type);
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.GetIndexParameters().Length == 0))
            {
                var currentPath = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";
                paths.Add(currentPath);

                if (IsNavigationProperty(prop.PropertyType))
                {
                    GetPropertyPaths(prop.PropertyType, currentPath, paths, maxDepth - 1, new HashSet<Type>(visited));
                }
                else if (IsCollectionProperty(prop.PropertyType))
                {
                    var elementType = GetCollectionElementType(prop.PropertyType);
                    if (elementType != null && IsNavigationProperty(elementType))
                    {
                        GetPropertyPaths(elementType, currentPath, paths, maxDepth - 1, new HashSet<Type>(visited));
                    }
                }
            }

            visited.Remove(type);
        }
    }
}