using UnitConversion.Converters;
using UnitConversion.Exceptions;

namespace UnitConversion.Tests.Converters;

public class TimeConverterTests
{
    [Fact]
    public void Convert_Hours_To_Seconds_Should_Return_Correct_Value()
    {
        var converter = new TimeConverter();

        var result = converter.Convert(1, "hours", "seconds");

        Assert.Equal(3600, result, 5);
    }

    [Fact]
    public void Convert_Days_To_Hours_Should_Return_Correct_Value()
    {
        var converter = new TimeConverter();

        var result = converter.Convert(1, "days", "hours");

        Assert.Equal(24, result, 5);
    }

    [Fact]
    public void Convert_Minutes_To_Seconds_Should_Return_Correct_Value()
    {
        var converter = new TimeConverter();

        var result = converter.Convert(5, "minutes", "seconds");

        Assert.Equal(300, result, 5);
    }

    [Fact]
    public void Convert_Invalid_Unit_Should_Throw_Exception()
    {
        var converter = new TimeConverter();

        Assert.Throws<InvalidUnitException>(() =>
            converter.Convert(1, "invalid", "hours"));
        Assert.Throws<InvalidUnitException>(() =>
            converter.Convert(1, "hours", "invalid"));
    }
}
