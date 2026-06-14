using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using UnitConversion.Constants;
using UnitConversion.Models.Requests;

namespace UnitConversion.Swagger;

public class ConversionSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != typeof(ConversionRequest))
            return;

        if (schema.Properties.TryGetValue("category", out var categorySchema))
        {
            categorySchema.Description = SwaggerDescriptions.BuildCategoryPropertyDescription();
            categorySchema.Enum = SupportedUnits.All
                .Select(category => new OpenApiString(category.Name))
                .Cast<IOpenApiAny>()
                .ToList();
            categorySchema.Example = new OpenApiString("length");
        }

        if (schema.Properties.TryGetValue("fromUnit", out var fromUnitSchema))
        {
            fromUnitSchema.Description = SwaggerDescriptions.BuildFromUnitPropertyDescription();
            fromUnitSchema.Example = new OpenApiString("kilometers");
        }

        if (schema.Properties.TryGetValue("toUnit", out var toUnitSchema))
        {
            toUnitSchema.Description = SwaggerDescriptions.BuildToUnitPropertyDescription();
            toUnitSchema.Example = new OpenApiString("miles");
        }

        if (schema.Properties.TryGetValue("value", out var valueSchema))
        {
            valueSchema.Description = "Numeric value to convert.";
            valueSchema.Example = new OpenApiDouble(1);
        }
    }
}
