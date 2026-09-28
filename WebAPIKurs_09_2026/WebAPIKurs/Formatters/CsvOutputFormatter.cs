using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace WebAPIKurs.Formatters;

public sealed class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        return type is not null && type != typeof(string) && CsvFormatterSupport.GetElementType(type) != typeof(object);
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var model = context.Object;
        var modelType = context.ObjectType ?? model?.GetType() ?? typeof(object);
        var elementType = CsvFormatterSupport.GetElementType(modelType);
        var properties = CsvFormatterSupport.GetProperties(elementType, writable: false);
        var values = CsvFormatterSupport.IsEnumerable(modelType) && model is IEnumerable enumerable
            ? enumerable.Cast<object?>()
            : new[] { model };

        await using var writer = new StreamWriter(context.HttpContext.Response.Body, selectedEncoding, leaveOpen: true);
        await writer.WriteLineAsync(string.Join(',', properties.Select(property => CsvFormatterSupport.Escape(property.Name))));
        foreach (var value in values)
        {
            var fields = properties.Select(property =>
            {
                var propertyValue = value is null ? null : property.GetValue(value);
                return CsvFormatterSupport.Escape(Convert.ToString(propertyValue, CultureInfo.InvariantCulture));
            });
            await writer.WriteLineAsync(string.Join(',', fields));
        }

        await writer.FlushAsync();
    }
}
