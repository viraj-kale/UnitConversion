public class AreaConverter : FactorUnitConverter
{
    public override string Category => "area";

    protected override IReadOnlyDictionary<string, double> ConversionFactors { get; } =
        new Dictionary<string, double>
        {
            { "squaremeter", 1 },
            { "squaremeters", 1 },
            { "squarefoot", 0.092903 },
            { "squarefeet", 0.092903 },
            { "acre", 4046.86 },
            { "acres", 4046.86 },
            { "hectare", 10000 },
            { "hectares", 10000 }
        };
}
