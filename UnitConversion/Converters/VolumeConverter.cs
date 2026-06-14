public class VolumeConverter : FactorUnitConverter
{
    public override string Category => "volume";

    protected override IReadOnlyDictionary<string, double> ConversionFactors { get; } =
        new Dictionary<string, double>
        {
            { "liter", 1 },
            { "liters", 1 },
            { "milliliter", 0.001 },
            { "milliliters", 0.001 },
            { "gallon", 3.78541 },
            { "gallons", 3.78541 },
            { "cup", 0.236588 },
            { "cups", 0.236588 }
        };
}
