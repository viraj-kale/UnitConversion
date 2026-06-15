namespace UnitConversion.Converters;

public class SpeedConverter : FactorUnitConverter
{
    public override string Category => "speed";

    protected override IReadOnlyDictionary<string, double> ConversionFactors { get; } =
        new Dictionary<string, double>
        {
            { "meterpersecond", 1 },
            { "meterspersecond", 1 },
            { "kilometerperhour", 0.277778 },
            { "kilometersperhour", 0.277778 },
            { "mileperhour", 0.44704 },
            { "milesperhour", 0.44704 }
        };
}
