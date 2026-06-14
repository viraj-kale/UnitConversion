public class WeightConverter : FactorUnitConverter
{
    public override string Category => "weight";

    protected override IReadOnlyDictionary<string, double> ConversionFactors { get; } =
        new Dictionary<string, double>
        {
            { "gram", 1 },
            { "grams", 1 },
            { "kilogram", 1000 },
            { "kilograms", 1000 },
            { "pound", 453.592 },
            { "pounds", 453.592 }
        };
}
