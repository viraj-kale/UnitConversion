using UnitConversion.Models.Requests;
using UnitConversion.Models.Responses;
using UnitConversion.Validators;

public class ConversionService : IConversionService
{
    private readonly IEnumerable<IUnitConverter> _converters;
    private readonly ConversionRequestValidator _validator;

    public ConversionService(
        IEnumerable<IUnitConverter> converters,
        ConversionRequestValidator validator)
    {
        _converters = converters;
        _validator = validator;
    }

    public ConversionResponse Convert(ConversionRequest request)
    {
        _validator.Validate(request);

        var converter = _converters.First(
            candidate => candidate.Category.Equals(
                request.Category,
                StringComparison.OrdinalIgnoreCase));

        double result = converter.Convert(
            request.Value!.Value,
            request.FromUnit,
            request.ToUnit);

        return new ConversionResponse
        {
            OriginalValue = request.Value.Value,
            FromUnit = request.FromUnit,
            ToUnit = request.ToUnit,
            ConvertedValue = Math.Round(result, 4)
        };
    }
}
