using UnitConversion.Constants;
using UnitConversion.Exceptions;

namespace UnitConversion.Converters;

public class TemperatureConverter : IUnitConverter
{
    private static readonly HashSet<string> ValidUnits =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "celsius",
            "fahrenheit",
            "kelvin"
        };

    public string Category => "temperature";

    public IReadOnlyCollection<string> SupportedUnits =>
        ValidUnits.ToArray();

    public bool IsValidUnit(string unit) =>
        ValidUnits.Contains(unit.Trim());

    public double Convert(
        double value,
        string fromUnit,
        string toUnit)
    {
        if (!IsValidUnit(fromUnit))
        {
            throw new InvalidUnitException(
                fromUnit,
                Category,
                GetDocumentedUnits(),
                "fromUnit");
        }

        if (!IsValidUnit(toUnit))
        {
            throw new InvalidUnitException(
                toUnit,
                Category,
                GetDocumentedUnits(),
                "toUnit");
        }

        double celsius = fromUnit.ToLower() switch
        {
            "celsius" => value,
            "fahrenheit" => (value - 32) * 5 / 9,
            "kelvin" => value - 273.15,
            _ => throw new InvalidUnitException(
                fromUnit,
                Category,
                GetDocumentedUnits(),
                "fromUnit")
        };

        return toUnit.ToLower() switch
        {
            "celsius" => celsius,
            "fahrenheit" => (celsius * 9 / 5) + 32,
            "kelvin" => celsius + 273.15,
            _ => throw new InvalidUnitException(
                toUnit,
                Category,
                GetDocumentedUnits(),
                "toUnit")
        };
    }

    private static IEnumerable<string> GetDocumentedUnits()
    {
        return UnitConversion.Constants.SupportedUnits.Categories.TryGetValue("temperature", out var units)
            ? units
            : [];
    }
}
