using System.Reflection;

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

            var validPropertyPaths = GetValidPropertyPaths<TDto>();

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

                    // Note: Collections cannot be sorted directly, only their properties can be used in filtering
                    if (IsCollectionProperty<TDto>(column))
                    {
                        errors.Add($"Cannot sort by collection property '{column}'. Sorting on collections is not supported.");
                        continue;
                    }

                    // Validate property path (supports dot notation)
                    if (!validPropertyPaths.Contains(column))
                        errors.Add($"Sort field '{column}' is not valid. Use dot notation for navigation properties (e.g., 'Company.Name').");

                    if (dir is not "asc" and not "desc")
                        errors.Add($"Sort direction '{dir}' is not valid. Use 'asc' or 'desc'.");
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
                        errors.Add($"Invalid filter operator in: '{raw}'. Supported operators: {string.Join(", ", operators)}");
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

                    if (!validPropertyPaths.Contains(field))
                        errors.Add($"Filter field '{field}' is not valid. Use dot notation for navigation properties (e.g., 'Services.Name' for collections).");

                    if (validPropertyPaths.Contains(field))
                    {
                        var validationError = ValidateFilterValue<TDto>(field, op, value);
                        if (validationError != null)
                            errors.Add(validationError);
                    }
                }
            }

            return errors;
        }

        private static HashSet<string> GetValidPropertyPaths<TDto>(int maxDepth = 3)
        {
            var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            GetPropertyPaths(typeof(TDto), "", paths, maxDepth, new HashSet<Type>());
            return paths;
        }

        private static void GetPropertyPaths(Type type, string prefix, HashSet<string> paths, int maxDepth, HashSet<Type> visitedTypes)
        {
            if (maxDepth <= 0 || visitedTypes.Contains(type))
                return;

            visitedTypes.Add(type);

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.GetIndexParameters().Length == 0); // Exclude indexers

            foreach (var prop in properties)
            {
                var currentPath = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";
                paths.Add(currentPath);

                // Handle navigation properties (complex types)
                if (IsNavigationProperty(prop.PropertyType))
                {
                    GetPropertyPaths(prop.PropertyType, currentPath, paths, maxDepth - 1, new HashSet<Type>(visitedTypes));
                }
                // Handle collection properties
                else if (IsCollectionProperty(prop.PropertyType))
                {
                    var elementType = GetCollectionElementType(prop.PropertyType);
                    if (elementType != null && IsNavigationProperty(elementType))
                    {
                        // Add collection element properties (e.g., Services.Name, Services.Id)
                        GetPropertyPaths(elementType, currentPath, paths, maxDepth - 1, new HashSet<Type>(visitedTypes));
                    }
                }
            }

            visitedTypes.Remove(type);
        }

        private static bool IsNavigationProperty(Type propertyType)
        {
            // Skip primitive types, strings, and common value types
            if (propertyType.IsPrimitive ||
                propertyType == typeof(string) ||
                propertyType == typeof(DateTime) ||
                propertyType == typeof(DateTimeOffset) ||
                propertyType == typeof(TimeSpan) ||
                propertyType == typeof(Guid) ||
                propertyType == typeof(decimal) ||
                Nullable.GetUnderlyingType(propertyType) != null)
            {
                return false;
            }

            // Skip collections (handled separately)
            if (IsCollectionProperty(propertyType))
            {
                return false;
            }

            // It's likely a navigation property (complex type)
            return propertyType.IsClass && propertyType != typeof(string);
        }

        private static bool IsCollectionProperty(Type propertyType)
        {
            if (propertyType == typeof(string)) return false; // String is IEnumerable but not a collection in our context

            return typeof(System.Collections.IEnumerable).IsAssignableFrom(propertyType) &&
                   propertyType != typeof(string);
        }

        private static Type? GetCollectionElementType(Type collectionType)
        {
            if (collectionType.IsGenericType)
            {
                var genericArgs = collectionType.GetGenericArguments();
                if (genericArgs.Length > 0)
                    return genericArgs [0];
            }

            // Handle non-generic IEnumerable
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(collectionType))
            {
                var enumerableInterface = collectionType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType &&
                                   i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                if (enumerableInterface != null)
                {
                    return enumerableInterface.GetGenericArguments() [0];
                }
            }

            return null;
        }

        private static bool IsCollectionProperty<TDto>(string propertyPath)
        {
            try
            {
                var type = typeof(TDto);
                var parts = propertyPath.Split('.');

                for (int i = 0; i < parts.Length; i++)
                {
                    var prop = type.GetProperty(parts [i], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (prop == null) return false;

                    // If this is the last part and it's a collection, return true
                    if (i == parts.Length - 1)
                        return IsCollectionProperty(prop.PropertyType);

                    // If it's a collection but not the last part, get the element type
                    if (IsCollectionProperty(prop.PropertyType))
                    {
                        type = GetCollectionElementType(prop.PropertyType);
                        if (type == null) return false;
                    }
                    else
                    {
                        type = prop.PropertyType;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private static string? ValidateFilterValue<TDto>(string propertyPath, string op, string value)
        {
            try
            {
                var propertyType = GetPropertyTypeFromPath<TDto>(propertyPath);
                if (propertyType == null) return null;

                // Handle nullable types
                var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

                // Skip validation for string properties
                if (underlyingType == typeof(string))
                    return null;

                // Validate numeric operations
                if (new [] { ">", ">=", "<", "<=" }.Contains(op))
                {
                    if (!IsNumericType(underlyingType) && underlyingType != typeof(DateTime) && underlyingType != typeof(DateTimeOffset))
                        return $"Operator '{op}' can only be used with numeric, date, or time properties for field '{propertyPath}'.";
                }

                if (op == "~=")
                {
                    if (underlyingType != typeof(string))
                        return $"Contains operator '~=' can only be used with string properties for field '{propertyPath}'.";
                }

                // Try to parse the value to ensure it's compatible with the property type
                if (underlyingType == typeof(int) && !int.TryParse(value, out _))
                    return $"Value '{value}' is not a valid integer for field '{propertyPath}'.";

                if (underlyingType == typeof(long) && !long.TryParse(value, out _))
                    return $"Value '{value}' is not a valid long integer for field '{propertyPath}'.";

                if (underlyingType == typeof(decimal) && !decimal.TryParse(value, out _))
                    return $"Value '{value}' is not a valid decimal for field '{propertyPath}'.";

                if (underlyingType == typeof(double) && !double.TryParse(value, out _))
                    return $"Value '{value}' is not a valid double for field '{propertyPath}'.";

                if (underlyingType == typeof(float) && !float.TryParse(value, out _))
                    return $"Value '{value}' is not a valid float for field '{propertyPath}'.";

                if (underlyingType == typeof(bool) && !bool.TryParse(value, out _))
                    return $"Value '{value}' is not a valid boolean for field '{propertyPath}'. Use 'true' or 'false'.";

                if (underlyingType == typeof(DateTime) && !DateTime.TryParse(value, out _))
                    return $"Value '{value}' is not a valid datetime for field '{propertyPath}'.";

                if (underlyingType == typeof(Guid) && !Guid.TryParse(value, out _))
                    return $"Value '{value}' is not a valid GUID for field '{propertyPath}'.";

                return null; // Validation passed
            }
            catch
            {
                return $"Unable to validate value '{value}' for field '{propertyPath}'.";
            }
        }

        private static bool IsNumericType(Type type)
        {
            return type == typeof(int) || type == typeof(long) || type == typeof(decimal) ||
                   type == typeof(double) || type == typeof(float) || type == typeof(short) ||
                   type == typeof(byte) || type == typeof(sbyte) || type == typeof(uint) ||
                   type == typeof(ulong) || type == typeof(ushort);
        }

        private static Type? GetPropertyTypeFromPath<TDto>(string propertyPath)
        {
            var type = typeof(TDto);
            var parts = propertyPath.Split('.');

            foreach (var part in parts)
            {
                var prop = type.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) return null;

                // If it's a collection, get the element type
                if (IsCollectionProperty(prop.PropertyType))
                {
                    type = GetCollectionElementType(prop.PropertyType);
                    if (type == null) return null;
                }
                else
                {
                    type = prop.PropertyType;
                }
            }

            return type;
        }
    }
}