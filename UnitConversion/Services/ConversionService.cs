using UnitConversion.Converters;
using UnitConversion.Exceptions;
using UnitConversion.Models.Requests;
using UnitConversion.Models.Responses;
using UnitConversion.Services.Interfaces;
using UnitConversion.Validators;

namespace UnitConversion.Services;

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

        var converter = _converters.FirstOrDefault(
            candidate => candidate.Category.Equals(
                request.Category,
                StringComparison.OrdinalIgnoreCase));

        if (converter is null)
            throw new UnsupportedCategoryException(request.Category);

        double result = converter.Convert(
            request.Value!.Value,
            request.FromUnit,
            request.ToUnit);

        return new ConversionResponse
        {
            Category = request.Category,
            OriginalValue = request.Value.Value,
            FromUnit = request.FromUnit,
            ToUnit = request.ToUnit,
            ConvertedValue = Math.Round(result, 4)
        };
    }
}
