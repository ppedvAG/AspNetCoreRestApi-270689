using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPIKurs.Formatters;

public sealed class CsvSwaggerOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.RequestBody is not null)
        {
            AddCsvContent(operation.RequestBody.Content);
        }

        foreach (var response in operation.Responses.Values)
        {
            AddCsvContent(response.Content);
        }
    }

    private static void AddCsvContent(IDictionary<string, OpenApiMediaType> content)
    {
        if (content.Count == 0 || content.ContainsKey("text/csv"))
        {
            return;
        }

        var schema = content.Values.FirstOrDefault(mediaType => mediaType.Schema is not null)?.Schema;
        if (schema is not null)
        {
            content["text/csv"] = new OpenApiMediaType { Schema = schema };
        }
    }
}
