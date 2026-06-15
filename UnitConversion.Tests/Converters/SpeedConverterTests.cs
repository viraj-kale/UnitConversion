using UnitConversion.Converters;
using UnitConversion.Exceptions;

namespace UnitConversion.Tests.Converters;

public class SpeedConverterTests
{
    [Fact]
    public void Convert_KilometersPerHour_To_MetersPerSecond_Should_Return_Correct_Value()
    {
        var converter = new SpeedConverter();

        var result = converter.Convert(
            36,
            "kilometersperhour",
            "meterspersecond");

        Assert.Equal(10, result, 4);
    }

    [Fact]
    public void Convert_MilesPerHour_To_KilometersPerHour_Should_Return_Correct_Value()
    {
        var converter = new SpeedConverter();

        var result = converter.Convert(
            60,
            "milesperhour",
            "kilometersperhour");

        Assert.Equal(96.56064, result, 4);
    }

    [Fact]
    public void Convert_MetersPerSecond_To_MilesPerHour_Should_Return_Correct_Value()
    {
        var converter = new SpeedConverter();

        var result = converter.Convert(
            1,
            "meterspersecond",
            "milesperhour");

        Assert.Equal(2.23694, result, 4);
    }

    [Fact]
    public void Convert_Invalid_Unit_Should_Throw_Exception()
    {
        var converter = new SpeedConverter();

        Assert.Throws<InvalidUnitException>(() =>
            converter.Convert(1, "invalid", "milesperhour"));
        Assert.Throws<InvalidUnitException>(() =>
            converter.Convert(1, "milesperhour", "invalid"));
    }
}
