using Domain.Abstractions;
using Shared.Utilities;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyQueryParamsAsync<T>(
        this IQueryable<T> query,
        PagedParameters queryParams)
    {
        var allowedPaths = GetValidPropertyPaths<T>();

        if (!string.IsNullOrWhiteSpace(queryParams.Filter))
        {
            var andGroups = queryParams.Filter.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var group in andGroups)
            {
                var orConditions = group.Split("||", StringSplitOptions.RemoveEmptyEntries);

                var expressions = new List<string>();
                var values = new List<object>();

                for (int i = 0; i < orConditions.Length; i++)
                {
                    var raw = orConditions [i];

                    var op = PagedResultUtils.Operators.FirstOrDefault(o => raw.Contains(o));
                    if (op == null)
                        continue;

                    var parts = raw.Split(op, 2);
                    if (parts.Length != 2)
                        continue;

                    var field = parts [0].Trim();
                    var value = parts [1].Trim();

                    if (!allowedPaths.Contains(field))
                        continue;

                    var expression = BuildFilterExpression<T>(field, op, value);
                    if (expression == null)
                        continue;

                    expressions.Add(expression.Replace("@0", $"@{values.Count}"));
                    values.Add(ConvertValueToPropertyType<T>(field, value));
                }

                if (expressions.Count > 0)
                {
                    query = query.Where(
                        $"({string.Join(" OR ", expressions)})",
                        values.ToArray());
                }
            }
        }
        // Sorting
        if (!string.IsNullOrWhiteSpace(queryParams.SortBy))
        {
            var sortParts = queryParams.SortBy
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(clause =>
                {
                    var parts = clause.Split(':');
                    var field = parts [0].Trim();
                    var direction = parts.Length == 2 && parts [1].Trim().ToLower() == "desc" ? "desc" : "asc";

                    return (field, direction);
                })
                .Where(s => allowedPaths.Contains(s.field) && !IsCollectionPath<T>(s.field))
                .Select(s => $"{s.field} {s.direction}")
                .ToList();

            if (sortParts.Count > 0)
                query = query.OrderBy(string.Join(", ", sortParts));
        }

        return query;
    }

    private static string? BuildFilterExpression<T>(string field, string op, string value)
    {
        if (IsCollectionPath<T>(field))
        {
            var path = GetCollectionPath<T>(field);
            if (path == null) return null;

            var (collection, member) = path.Value;
            return op switch
            {
                "==" => $"{collection}.Any({member} == @0)",
                "!=" => $"!{collection}.Any({member} == @0)",
                ">" => $"{collection}.Any({member} > @0)",
                ">=" => $"{collection}.Any({member} >= @0)",
                "<" => $"{collection}.Any({member} < @0)",
                "<=" => $"{collection}.Any({member} <= @0)",
                "~=" => $"{collection}.Any({member} != null && {member}.Contains(@0))",
                _ => null
            };
        }

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

    private static (string Collection, string Member)? GetCollectionPath<T>(string propertyPath)
    {
        var parts = propertyPath.Split('.');
        if (parts.Length < 2) return null;

        var type = typeof(T);
        var path = "";

        for (int i = 0; i < parts.Length - 1; i++)
        {
            var prop = type.GetProperty(parts [i], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop == null) return null;

            type = prop.PropertyType;
            path = string.IsNullOrEmpty(path) ? parts [i] : $"{path}.{parts [i]}";

            if (IsCollection(prop.PropertyType))
            {
                var member = string.Join('.', parts.Skip(i + 1));
                return (path, member);
            }
        }

        return null;
    }

    private static bool IsCollectionPath<T>(string propertyPath)
    {
        var type = typeof(T);
        foreach (var part in propertyPath.Split('.'))
        {
            var prop = type.GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop == null) return false;

            if (IsCollection(prop.PropertyType)) return true;

            type = prop.PropertyType;
        }

        return false;
    }

    private static HashSet<string> GetValidPropertyPaths<T>(int depth = 3)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        TraverseProperties(typeof(T), "", result, depth, new HashSet<Type>());
        return result;
    }

    private static void TraverseProperties(Type type, string prefix, HashSet<string> paths, int depth, HashSet<Type> visited)
    {
        if (depth <= 0 || visited.Contains(type)) return;

        visited.Add(type);

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(p => p.CanRead && p.GetIndexParameters().Length == 0))
        {
            var current = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";
            paths.Add(current);

            if (IsComplex(prop.PropertyType))
                TraverseProperties(prop.PropertyType, current, paths, depth - 1, new HashSet<Type>(visited));
            else if (IsCollection(prop.PropertyType))
            {
                var elementType = GetElementType(prop.PropertyType);
                if (elementType != null && IsComplex(elementType))
                    TraverseProperties(elementType, current, paths, depth - 1, new HashSet<Type>(visited));
            }
        }

        visited.Remove(type);
    }

    private static bool IsCollection(Type type) =>
        type != typeof(string) && typeof(System.Collections.IEnumerable).IsAssignableFrom(type);

    private static bool IsComplex(Type type) =>
        !type.IsPrimitive &&
        type != typeof(string) &&
        type != typeof(decimal) &&
        Nullable.GetUnderlyingType(type) == null &&
        !type.IsEnum;

    private static Type? GetElementType(Type collectionType)
    {
        if (collectionType.IsGenericType)
            return collectionType.GetGenericArguments().FirstOrDefault();

        return collectionType.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            ?.GetGenericArguments().FirstOrDefault();
    }

    private static object ConvertValueToPropertyType<T>(string propertyPath, string value)
    {
        try
        {
            var type = GetPropertyType<T>(propertyPath);
            if (type == null) return value;

            var underlying = Nullable.GetUnderlyingType(type) ?? type;
            var isNullable = Nullable.GetUnderlyingType(type) != null;

            if (value.Equals("null", StringComparison.OrdinalIgnoreCase))
                return isNullable ? null! : throw new ArgumentException($"Field '{propertyPath}' is not nullable.");

            return Type.GetTypeCode(underlying) switch
            {
                TypeCode.Int32 => int.Parse(value),
                TypeCode.Int64 => long.Parse(value),
                TypeCode.Decimal => decimal.Parse(value),
                TypeCode.Double => double.Parse(value),
                TypeCode.Single => float.Parse(value),
                TypeCode.Boolean => bool.Parse(value),
                TypeCode.DateTime => DateTime.Parse(value),
                TypeCode.String => value,
                _ when underlying == typeof(Guid) => Guid.Parse(value),
                _ => Convert.ChangeType(value, underlying)
            };
        }
        catch
        {
            return value;
        }
    }

    private static Type? GetPropertyType<T>(string propertyPath)
    {
        var type = typeof(T);
        foreach (var part in propertyPath.Split('.'))
        {
            var prop = type.GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop == null) return null;

            if (IsCollection(prop.PropertyType))
            {
                var elementType = GetElementType(prop.PropertyType);
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
}
