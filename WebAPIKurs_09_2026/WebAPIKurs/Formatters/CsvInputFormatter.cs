using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace WebAPIKurs.Formatters;

public sealed class CsvInputFormatter : TextInputFormatter
{
    public CsvInputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanReadType(Type type)
    {
        return type != typeof(string) && CsvFormatterSupport.GetElementType(type) != typeof(object);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    {
        try
        {
            using var reader = new StreamReader(context.HttpContext.Request.Body, encoding);
            var rows = CsvFormatterSupport.Parse(new StringReader(await reader.ReadToEndAsync()));
            if (rows.Count == 0)
            {
                return await InputFormatterResult.NoValueAsync();
            }

            var modelType = context.ModelType;
            var elementType = CsvFormatterSupport.GetElementType(modelType);
            var properties = CsvFormatterSupport.GetProperties(elementType, writable: true);
            var headers = rows[0];
            var propertyIndexes = properties.ToDictionary(
                property => property,
                property => Array.FindIndex(headers, header => string.Equals(header.Trim(), property.Name, StringComparison.OrdinalIgnoreCase)));

            if (propertyIndexes.Values.Any(index => index < 0))
            {
                var missing = string.Join(", ", propertyIndexes.Where(item => item.Value < 0).Select(item => item.Key.Name));
                context.ModelState.AddModelError(string.Empty, $"CSV-Spalten fehlen: {missing}.");
                return await InputFormatterResult.FailureAsync();
            }

            var values = rows.Skip(1)
                .Select(row => CreateObject(elementType, properties, propertyIndexes, row, context))
                .ToList();

            if (!CsvFormatterSupport.IsEnumerable(modelType))
            {
                return await InputFormatterResult.SuccessAsync(values.FirstOrDefault());
            }

            if (modelType.IsArray)
            {
                var array = Array.CreateInstance(elementType, values.Count);
                for (var index = 0; index < values.Count; index++) array.SetValue(values[index], index);
                return await InputFormatterResult.SuccessAsync(array);
            }

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType))!;
            foreach (var value in values) list.Add(value);
            return await InputFormatterResult.SuccessAsync(list);
        }
        catch (Exception exception) when (exception is FormatException or ArgumentException or InvalidCastException or OverflowException)
        {
            context.ModelState.AddModelError(string.Empty, $"CSV konnte nicht gelesen werden: {exception.Message}");
            return await InputFormatterResult.FailureAsync();
        }
    }

    private static object CreateObject(
        Type elementType,
        PropertyInfo[] properties,
        IReadOnlyDictionary<PropertyInfo, int> propertyIndexes,
        string[] row,
        InputFormatterContext context)
    {
        var instance = Activator.CreateInstance(elementType)
            ?? throw new InvalidOperationException($"Für {elementType.Name} ist kein parameterloser Konstruktor vorhanden.");

        foreach (var property in properties)
        {
            var index = propertyIndexes[property];
            var value = index < row.Length ? row[index] : string.Empty;
            try
            {
                property.SetValue(instance, CsvFormatterSupport.ConvertValue(value, property.PropertyType));
            }
            catch (Exception exception) when (exception is FormatException or ArgumentException or InvalidCastException or OverflowException)
            {
                context.ModelState.AddModelError(property.Name, $"Der Wert '{value}' ist für {property.PropertyType.Name} ungültig.");
                throw;
            }
        }

        return instance;
    }
}
