using UnitConversion.Exceptions;
using UnitConversion.Models.Requests;
using UnitConversion.Validators;

namespace UnitConversion.Tests.Validators;

public class ConversionRequestValidatorTests
{
    private readonly ConversionRequestValidator _validator = new(
    [
        new LengthConverter(),
        new TemperatureConverter(),
        new WeightConverter(),
        new VolumeConverter(),
        new AreaConverter(),
        new SpeedConverter(),
        new TimeConverter()
    ]);

    [Fact]
    public void Validate_MissingFields_ThrowsValidationApiException()
    {
        var request = new ConversionRequest();

        var exception = Assert.Throws<ValidationApiException>(
            () => _validator.Validate(request));

        Assert.Equal("VALIDATION_ERROR", exception.ErrorCode);
        Assert.Contains("category", exception.Errors.Keys);
        Assert.Contains("fromUnit", exception.Errors.Keys);
        Assert.Contains("toUnit", exception.Errors.Keys);
        Assert.Contains("value", exception.Errors.Keys);
    }

    [Fact]
    public void Validate_UnsupportedCategory_ThrowsUnsupportedCategoryException()
    {
        var request = new ConversionRequest
        {
            Category = "distance",
            FromUnit = "meters",
            ToUnit = "feet",
            Value = 1
        };

        var exception = Assert.Throws<UnsupportedCategoryException>(
            () => _validator.Validate(request));

        Assert.Equal("UNSUPPORTED_CATEGORY", exception.ErrorCode);
        Assert.Contains("length", (string[])exception.Extensions!["supportedCategories"]!);
    }

    [Fact]
    public void Validate_InvalidFromUnit_ThrowsInvalidUnitException()
    {
        var request = new ConversionRequest
        {
            Category = "length",
            FromUnit = "yards",
            ToUnit = "meters",
            Value = 1
        };

        var exception = Assert.Throws<InvalidUnitException>(
            () => _validator.Validate(request));

        Assert.Equal("INVALID_UNIT", exception.ErrorCode);
        Assert.Equal("fromUnit", exception.Extensions!["unitRole"]);
    }

    [Fact]
    public void Validate_InvalidToUnit_ThrowsInvalidUnitException()
    {
        var request = new ConversionRequest
        {
            Category = "temperature",
            FromUnit = "celsius",
            ToUnit = "rankine",
            Value = 100
        };

        var exception = Assert.Throws<InvalidUnitException>(
            () => _validator.Validate(request));

        Assert.Equal("INVALID_UNIT", exception.ErrorCode);
        Assert.Equal("toUnit", exception.Extensions!["unitRole"]);
    }

    [Fact]
    public void Validate_NonFiniteValue_ThrowsValidationApiException()
    {
        var request = new ConversionRequest
        {
            Category = "length",
            FromUnit = "meters",
            ToUnit = "feet",
            Value = double.PositiveInfinity
        };

        var exception = Assert.Throws<ValidationApiException>(
            () => _validator.Validate(request));

        Assert.Contains("value", exception.Errors.Keys);
    }

    [Fact]
    public void Validate_ValidRequest_DoesNotThrow()
    {
        var request = new ConversionRequest
        {
            Category = "weight",
            FromUnit = "kilograms",
            ToUnit = "pounds",
            Value = 2.5
        };

        _validator.Validate(request);
    }
}
