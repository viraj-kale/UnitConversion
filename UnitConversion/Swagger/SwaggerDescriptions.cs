using System.Text;
using UnitConversion.Constants;

namespace UnitConversion.Swagger;

public static class SwaggerDescriptions
{
    public const string ApiTitle = "Unit Conversion API";

    public const string ApiVersion = "v1";

    public static string BuildApiDescription()
    {
        var builder = new StringBuilder();

        builder.AppendLine("Convert values between units within a supported category.");
        builder.AppendLine();
        builder.AppendLine("Send a **POST** request with the category, source unit, target unit, and numeric value.");
        builder.AppendLine("Unit names are **case-insensitive**. Singular and plural forms are accepted for most categories (e.g. `meter` / `meters`, `foot` / `feet`).");
        builder.AppendLine();
        builder.AppendLine("## Supported conversion types");
        builder.AppendLine();

        foreach (var category in SupportedUnits.All)
        {
            builder.AppendLine($"### {category.Name}");
            builder.AppendLine(category.Description);
            builder.AppendLine();
            builder.AppendLine($"**Units:** `{string.Join("`, `", category.Units)}`");
            builder.AppendLine();
        }

        builder.AppendLine("## Example requests");
        builder.AppendLine();
        builder.AppendLine("- **length:** 1 `kilometer` → `miles`");
        builder.AppendLine("- **weight:** 2.5 `kilograms` → `pounds`");
        builder.AppendLine("- **temperature:** 100 `celsius` → `fahrenheit`");
        builder.AppendLine("- **volume:** 1 `gallon` → `liters`");
        builder.AppendLine("- **area:** 1 `acre` → `squarefeet`");
        builder.AppendLine("- **speed:** 60 `milesperhour` → `kilometersperhour`");
        builder.AppendLine("- **time:** 2 `hours` → `minutes`");

        return builder.ToString().TrimEnd();
    }

    public static string BuildCategoryPropertyDescription()
    {
        var lines = SupportedUnits.All
            .Select(category =>
                $"- **{category.Name}** — {string.Join(", ", category.Units)}");

        return "Conversion category. Supported values:\n" +
               string.Join('\n', lines);
    }

    public static string BuildFromUnitPropertyDescription()
    {
        return "Source unit to convert from. Must belong to the selected category. " +
               "See the API description above for the full list of units per category.";
    }

    public static string BuildToUnitPropertyDescription()
    {
        return "Target unit to convert to. Must belong to the selected category. " +
               "See the API description above for the full list of units per category.";
    }
}
