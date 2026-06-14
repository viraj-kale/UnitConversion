using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UnitConversion.Swagger;

public class ConversionExamplesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.Name != "Convert")
            return;

        if (operation.RequestBody?.Content is null ||
            !operation.RequestBody.Content.TryGetValue("application/json", out var mediaType))
            return;

        mediaType.Examples = new Dictionary<string, OpenApiExample>
        {
            ["length — kilometers to miles"] = CreateExample(
                "length", "kilometers", "miles", 1),
            ["weight — kilograms to pounds"] = CreateExample(
                "weight", "kilograms", "pounds", 2.5),
            ["temperature — celsius to fahrenheit"] = CreateExample(
                "temperature", "celsius", "fahrenheit", 100),
            ["volume — gallons to liters"] = CreateExample(
                "volume", "gallons", "liters", 1),
            ["area — acres to square feet"] = CreateExample(
                "area", "acres", "squarefeet", 1),
            ["speed — mph to km/h"] = CreateExample(
                "speed", "milesperhour", "kilometersperhour", 60),
            ["time — hours to minutes"] = CreateExample(
                "time", "hours", "minutes", 2)
        };
    }

    private static OpenApiExample CreateExample(
        string category,
        string fromUnit,
        string toUnit,
        double value)
    {
        return new OpenApiExample
        {
            Value = new OpenApiObject
            {
                ["category"] = new OpenApiString(category),
                ["fromUnit"] = new OpenApiString(fromUnit),
                ["toUnit"] = new OpenApiString(toUnit),
                ["value"] = new OpenApiDouble(value)
            }
        };
    }
}
