using UnitConversion.Constants;
using UnitConversion.Exceptions;
using UnitConversion.Models.Requests;

namespace UnitConversion.Validators;

public class ConversionRequestValidator
{
    private readonly IEnumerable<IUnitConverter> _converters;

    public ConversionRequestValidator(IEnumerable<IUnitConverter> converters)
    {
        _converters = converters;
    }

    public void Validate(ConversionRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.Category))
            errors["category"] = ["Category is required."];

        if (string.IsNullOrWhiteSpace(request.FromUnit))
            errors["fromUnit"] = ["FromUnit is required."];

        if (string.IsNullOrWhiteSpace(request.ToUnit))
            errors["toUnit"] = ["ToUnit is required."];

        if (request.Value is null)
            errors["value"] = ["Value is required."];
        else if (double.IsNaN(request.Value.Value) || double.IsInfinity(request.Value.Value))
            errors["value"] = ["Value must be a finite number."];

        if (errors.Count > 0)
            throw new ValidationApiException(errors);

        var converter = _converters.FirstOrDefault(
            candidate => candidate.Category.Equals(
                request.Category,
                StringComparison.OrdinalIgnoreCase));

        if (converter is null)
            throw new UnsupportedCategoryException(request.Category);

        if (!converter.IsValidUnit(request.FromUnit))
        {
            throw new InvalidUnitException(
                request.FromUnit,
                converter.Category,
                GetDocumentedUnits(converter.Category),
                "fromUnit");
        }

        if (!converter.IsValidUnit(request.ToUnit))
        {
            throw new InvalidUnitException(
                request.ToUnit,
                converter.Category,
                GetDocumentedUnits(converter.Category),
                "toUnit");
        }
    }

    private static IEnumerable<string> GetDocumentedUnits(string category)
    {
        return SupportedUnits.Categories.TryGetValue(category, out var units)
            ? units
            : [];
    }
}
