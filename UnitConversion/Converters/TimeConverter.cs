namespace UnitConversion.Converters;

public class TimeConverter : FactorUnitConverter
{
    public override string Category => "time";

    protected override IReadOnlyDictionary<string, double> ConversionFactors { get; } =
        new Dictionary<string, double>
        {
            { "second", 1 },
            { "seconds", 1 },
            { "minute", 60 },
            { "minutes", 60 },
            { "hour", 3600 },
            { "hours", 3600 },
            { "day", 86400 },
            { "days", 86400 }
        };
}
