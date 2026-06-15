using UnitConversion.Converters;
using UnitConversion.Exceptions;

namespace UnitConversion.Tests.Converters;

public class VolumeConverterTests
{
    [Fact]
    public void Convert_Liters_To_Milliliters_Should_Return_Correct_Value()
    {
        var converter = new VolumeConverter();

        var result = converter.Convert(1, "liters", "milliliters");

        Assert.Equal(1000, result, 5);
    }

    [Fact]
    public void Convert_Gallons_To_Liters_Should_Return_Correct_Value()
    {
        var converter = new VolumeConverter();

        var result = converter.Convert(1, "gallons", "liters");

        Assert.Equal(3.78541, result, 5);
    }

    [Fact]
    public void Convert_Cups_To_Liters_Should_Return_Correct_Value()
    {
        var converter = new VolumeConverter();

        var result = converter.Convert(4, "cups", "liters");

        Assert.Equal(0.946352, result, 5);
    }

    [Fact]
    public void Convert_Invalid_Unit_Should_Throw_Exception()
    {
        var converter = new VolumeConverter();

        Assert.Throws<InvalidUnitException>(() =>
            converter.Convert(1, "invalid", "liters"));
        Assert.Throws<InvalidUnitException>(() =>
            converter.Convert(1, "liters", "invalid"));
    }
}
