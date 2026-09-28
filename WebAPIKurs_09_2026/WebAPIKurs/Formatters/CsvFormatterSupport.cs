using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace WebAPIKurs.Formatters;

internal static class CsvFormatterSupport
{
    public static PropertyInfo[] GetProperties(Type type, bool writable)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.CanRead && (!writable || property.CanWrite))
            .ToArray();
    }

    public static Type GetElementType(Type type)
    {
        if (type != typeof(string) && type.IsArray)
        {
            return type.GetElementType()!;
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            return type.GetGenericArguments()[0];
        }

        return type.GetInterfaces()
            .Where(interfaceType => interfaceType.IsGenericType)
            .Where(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            .Select(interfaceType => interfaceType.GetGenericArguments()[0])
            .FirstOrDefault() ?? type;
    }

    public static bool IsEnumerable(Type type)
    {
        return type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
    }

    public static string Escape(string? value)
    {
        value ??= string.Empty;
        if (!value.Contains(',', StringComparison.Ordinal) &&
            !value.Contains('"', StringComparison.Ordinal) &&
            !value.Contains('\r', StringComparison.Ordinal) &&
            !value.Contains('\n', StringComparison.Ordinal))
        {
            return value;
        }

        return $"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";
    }

    public static List<string[]> Parse(TextReader reader)
    {
        var rows = new List<string[]>();
        var row = new List<string>();
        var field = new StringBuilder();
        var quoted = false;

        while (reader.Read() is var value && value >= 0)
        {
            var character = (char)value;
            if (quoted)
            {
                if (character == '"')
                {
                    if (reader.Peek() == '"')
                    {
                        reader.Read();
                        field.Append('"');
                    }
                    else
                    {
                        quoted = false;
                    }
                }
                else
                {
                    field.Append(character);
                }
            }
            else if (character == '"' && field.Length == 0)
            {
                quoted = true;
            }
            else if (character == ',')
            {
                row.Add(field.ToString());
                field.Clear();
            }
            else if (character == '\r' || character == '\n')
            {
                if (character == '\r' && reader.Peek() == '\n')
                {
                    reader.Read();
                }

                row.Add(field.ToString());
                field.Clear();
                if (row.Any(value => value.Length > 0))
                {
                    rows.Add(row.ToArray());
                }

                row.Clear();
            }
            else
            {
                field.Append(character);
            }
        }

        if (quoted)
        {
            throw new FormatException("Die CSV-Eingabe enthält ein nicht geschlossenes Textfeld.");
        }

        if (field.Length > 0 || row.Count > 0)
        {
            row.Add(field.ToString());
            rows.Add(row.ToArray());
        }

        return rows;
    }

    public static object? ConvertValue(string value, Type targetType)
    {
        var nullableType = Nullable.GetUnderlyingType(targetType);
        var actualType = nullableType ?? targetType;
        if (string.IsNullOrEmpty(value))
        {
            if (nullableType is not null || !actualType.IsValueType)
            {
                return null;
            }

            return Activator.CreateInstance(actualType);
        }

        if (actualType == typeof(string)) return value;
        if (actualType == typeof(Guid)) return Guid.Parse(value);
        if (actualType == typeof(DateTime)) return DateTime.Parse(value, CultureInfo.InvariantCulture);
        if (actualType == typeof(DateTimeOffset)) return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture);
        if (actualType.IsEnum) return Enum.Parse(actualType, value, ignoreCase: true);
        return Convert.ChangeType(value, actualType, CultureInfo.InvariantCulture);
    }
}
