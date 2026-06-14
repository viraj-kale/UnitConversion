namespace UnitConversion.Constants;

public static class SupportedUnits
{
    public record CategoryInfo(
        string Name,
        string Description,
        IReadOnlyList<string> Units);

    public static readonly IReadOnlyList<CategoryInfo> All =
    [
        new(
            "length",
            "Distance and dimension measurements.",
            ["meter", "kilometer", "foot", "mile"]),
        new(
            "weight",
            "Mass measurements.",
            ["gram", "kilogram", "pound"]),
        new(
            "temperature",
            "Temperature scales (offset conversions, not simple factors).",
            ["celsius", "fahrenheit", "kelvin"]),
        new(
            "volume",
            "Liquid and capacity measurements (US gallon).",
            ["liter", "milliliter", "gallon", "cup"]),
        new(
            "area",
            "Surface and land area measurements.",
            ["squaremeter", "squarefoot", "acre", "hectare"]),
        new(
            "speed",
            "Rate of motion.",
            ["meterpersecond", "kilometerperhour", "mileperhour"]),
        new(
            "time",
            "Duration measurements.",
            ["second", "minute", "hour", "day"])
    ];

    public static readonly IReadOnlyDictionary<string, IReadOnlyList<string>> Categories =
        All.ToDictionary(
            category => category.Name,
            category => category.Units);
}
