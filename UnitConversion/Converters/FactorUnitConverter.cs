using UnitConversion.Constants;
using UnitConversion.Exceptions;

namespace UnitConversion.Converters;

public abstract class FactorUnitConverter : IUnitConverter
{
    public abstract string Category { get; }

    protected abstract IReadOnlyDictionary<string, double> ConversionFactors { get; }

    public IReadOnlyCollection<string> SupportedUnits =>
        ConversionFactors.Keys.ToArray();

    public bool IsValidUnit(string unit) =>
        TryGetFactor(unit, out _);

    public double Convert(double value, string fromUnit, string toUnit)
    {
        if (!TryGetFactor(fromUnit, out var fromFactor))
        {
            throw new InvalidUnitException(
                fromUnit,
                Category,
                GetDocumentedUnits(),
                "fromUnit");
        }

        if (!TryGetFactor(toUnit, out var toFactor))
        {
            throw new InvalidUnitException(
                toUnit,
                Category,
                GetDocumentedUnits(),
                "toUnit");
        }

        return value * fromFactor / toFactor;
    }

    protected bool TryGetFactor(string unit, out double factor)
    {
        var normalized = unit.Trim().ToLowerInvariant();
        return ConversionFactors.TryGetValue(normalized, out factor);
    }

    private IEnumerable<string> GetDocumentedUnits()
    {
        return UnitConversion.Constants.SupportedUnits.Categories.TryGetValue(Category, out var units)
            ? units
            : [];
    }
}
