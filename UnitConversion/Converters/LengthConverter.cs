public class LengthConverter : FactorUnitConverter
{
    public override string Category => "length";

    protected override IReadOnlyDictionary<string, double> ConversionFactors { get; } =
        new Dictionary<string, double>
        {
            { "meter", 1 },
            { "meters", 1 },
            { "kilometer", 1000 },
            { "kilometers", 1000 },
            { "foot", 0.3048 },
            { "feet", 0.3048 },
            { "mile", 1609.344 },
            { "miles", 1609.344 }
        };
}
