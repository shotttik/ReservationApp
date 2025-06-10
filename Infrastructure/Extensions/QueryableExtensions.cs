using Domain.Abstractions;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        private static readonly string [] SupportedOperators = ["==", "!=", ">=", "<=", ">", "<", "~="];

        public static IQueryable<T> ApplyQueryParamsAsync<T>(
            this IQueryable<T> query,
            PagedParameters queryParams)
        {
            var allowedPropertyPaths = GetValidPropertyPaths<T>();

            // Filtering
            if (!string.IsNullOrWhiteSpace(queryParams.Filter))
            {
                var filters = queryParams.Filter.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var raw in filters)
                {
                    // Parse operator from filter string
                    var op = SupportedOperators.FirstOrDefault(o => raw.Contains(o));
                    if (op == null) continue;

                    var parts = raw.Split(op, 2);
                    if (parts.Length != 2) continue;

                    var field = parts [0].Trim();
                    var value = parts [1].Trim();

                    if (!IsValidPropertyPath<T>(field, allowedPropertyPaths)) continue;

                    var expression = BuildFilterExpression<T>(field, op, value);

                    if (expression != null)
                    {
                        var convertedValue = ConvertValueToPropertyType<T>(field, value);
                        query = query.Where(expression, convertedValue);
                    }
                }
            }

            // Sorting (enhanced to support navigation properties, but not collections)
            if (!string.IsNullOrWhiteSpace(queryParams.SortBy))
            {
                var clauses = queryParams.SortBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var sortParts = new List<string>();

                foreach (var clause in clauses)
                {
                    var parts = clause.Split(':');
                    var field = parts [0].Trim();
                    var dir = parts.Length == 2 && parts [1].Trim().ToLower() == "desc" ? "desc" : "asc";

                    // Validate sorting field path (collections cannot be sorted)
                    if (IsValidPropertyPath<T>(field, allowedPropertyPaths) && !IsCollectionPropertyPath<T>(field))
                        sortParts.Add($"{field} {dir}");
                }

                if (sortParts.Count > 0)
                    query = query.OrderBy(string.Join(", ", sortParts));
            }

            return query;
        }

        private static string? BuildFilterExpression<T>(string field, string op, string value)
        {
            if (IsCollectionPropertyPath<T>(field))
            {
                // For collection properties, use Any() method
                // e.g., Services.Name == "test" becomes Services.Any(x => x.Name == @0)
                var collectionPath = GetCollectionPath<T>(field);
                if (collectionPath != null)
                {
                    var (collectionProperty, memberProperty) = collectionPath.Value;

                    return op switch
                    {
                        "==" => $"{collectionProperty}.Any({memberProperty} == @0)",
                        "!=" => $"!{collectionProperty}.Any({memberProperty} == @0)", // None match
                        ">" => $"{collectionProperty}.Any({memberProperty} > @0)",
                        ">=" => $"{collectionProperty}.Any({memberProperty} >= @0)",
                        "<" => $"{collectionProperty}.Any({memberProperty} < @0)",
                        "<=" => $"{collectionProperty}.Any({memberProperty} <= @0)",
                        "~=" => $"{collectionProperty}.Any({memberProperty} != null && {memberProperty}.Contains(@0))",
                        _ => null
                    };
                }
            }
            else
            {
                // Regular property filtering
                return op switch
                {
                    "==" => $"{field} == @0",
                    "!=" => $"{field} != @0",
                    ">" => $"{field} > @0",
                    ">=" => $"{field} >= @0",
                    "<" => $"{field} < @0",
                    "<=" => $"{field} <= @0",
                    "~=" => $"{field} != null && {field}.Contains(@0)",
                    _ => null
                };
            }

            return null;
        }

        private static (string CollectionProperty, string MemberProperty)? GetCollectionPath<T>(string propertyPath)
        {
            var parts = propertyPath.Split('.');
            if (parts.Length < 2) return null;

            var type = typeof(T);
            string collectionProperty = "";

            for (int i = 0; i < parts.Length - 1; i++)
            {
                var prop = type.GetProperty(parts [i], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) return null;

                collectionProperty = string.IsNullOrEmpty(collectionProperty) ? parts [i] : $"{collectionProperty}.{parts [i]}";

                if (IsCollectionProperty(prop.PropertyType))
                {
                    // Found the collection, the rest should be the member property path
                    var memberProperty = string.Join(".", parts.Skip(i + 1));
                    return (collectionProperty, memberProperty);
                }

                type = prop.PropertyType;
            }

            return null;
        }

        private static bool IsCollectionPropertyPath<T>(string propertyPath)
        {
            var parts = propertyPath.Split('.');
            var type = typeof(T);

            foreach (var part in parts)
            {
                var prop = type.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) return false;

                if (IsCollectionProperty(prop.PropertyType))
                    return true;

                type = prop.PropertyType;
            }

            return false;
        }

        private static HashSet<string> GetValidPropertyPaths<T>(int maxDepth = 2)
        {
            var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            GetPropertyPaths(typeof(T), "", paths, maxDepth, new HashSet<Type>());
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

                if (IsNavigationProperty(prop.PropertyType))
                {
                    GetPropertyPaths(prop.PropertyType, currentPath, paths, maxDepth - 1, new HashSet<Type>(visitedTypes));
                }
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

        private static bool IsValidPropertyPath<T>(string propertyPath, HashSet<string> allowedPaths)
        {
            return allowedPaths.Contains(propertyPath);
        }

        private static object ConvertValueToPropertyType<T>(string propertyPath, string value)
        {
            try
            {
                var propertyType = GetPropertyTypeFromPath<T>(propertyPath);
                if (propertyType == null) return value;

                // Handle nullable types
                var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

                if (underlyingType == typeof(string))
                    return value;

                if (underlyingType == typeof(int))
                    return int.Parse(value);

                if (underlyingType == typeof(long))
                    return long.Parse(value);

                if (underlyingType == typeof(decimal))
                    return decimal.Parse(value);

                if (underlyingType == typeof(double))
                    return double.Parse(value);

                if (underlyingType == typeof(float))
                    return float.Parse(value);

                if (underlyingType == typeof(bool))
                    return bool.Parse(value);

                if (underlyingType == typeof(DateTime))
                    return DateTime.Parse(value);

                if (underlyingType == typeof(Guid))
                    return Guid.Parse(value);

                // For other types, try Convert.ChangeType
                return Convert.ChangeType(value, underlyingType);
            }
            catch
            {
                // If conversion fails, return as string
                return value;
            }
        }

        private static Type? GetPropertyTypeFromPath<T>(string propertyPath)
        {
            var type = typeof(T);
            var parts = propertyPath.Split('.');

            foreach (var part in parts)
            {
                var prop = type.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) return null;

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