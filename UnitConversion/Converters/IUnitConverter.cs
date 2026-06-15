namespace UnitConversion.Converters;

public interface IUnitConverter
{
    string Category { get; }

    IReadOnlyCollection<string> SupportedUnits { get; }

    bool IsValidUnit(string unit);

    double Convert(double value, string fromUnit, string toUnit);
}
