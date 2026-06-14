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
            throw new InvalidOperationException($"Invalid source unit '{fromUnit}'.");

        if (!IsValidUnit(toUnit))
            throw new InvalidOperationException($"Invalid target unit '{toUnit}'.");

        double celsius = fromUnit.ToLower() switch
        {
            "celsius" => value,
            "fahrenheit" => (value - 32) * 5 / 9,
            "kelvin" => value - 273.15,
            _ => throw new InvalidOperationException($"Invalid source unit '{fromUnit}'.")
        };

        return toUnit.ToLower() switch
        {
            "celsius" => celsius,
            "fahrenheit" => (celsius * 9 / 5) + 32,
            "kelvin" => celsius + 273.15,
            _ => throw new InvalidOperationException($"Invalid target unit '{toUnit}'.")
        };
    }
}
