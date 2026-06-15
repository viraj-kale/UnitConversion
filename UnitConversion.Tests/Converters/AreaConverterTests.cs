using UnitConversion.Converters;
using UnitConversion.Exceptions;

namespace UnitConversion.Tests.Converters;

public class AreaConverterTests
{
    [Fact]
    public void Convert_Hectares_To_SquareMeters_Should_Return_Correct_Value()
    {
        var converter = new AreaConverter();

        var result = converter.Convert(1, "hectares", "squaremeters");

        Assert.Equal(10000, result, 5);
    }

    [Fact]
    public void Convert_Acres_To_SquareFeet_Should_Return_Correct_Value()
    {
        var converter = new AreaConverter();

        var result = converter.Convert(1, "acres", "squarefeet");

        Assert.Equal(43560, result, 0);
    }

    [Fact]
    public void Convert_SquareFeet_To_SquareMeters_Should_Return_Correct_Value()
    {
        var converter = new AreaConverter();

        var result = converter.Convert(10.7639, "squarefeet", "squaremeters");

        Assert.Equal(1, result, 4);
    }

    [Fact]
    public void Convert_Invalid_Unit_Should_Throw_Exception()
    {
        var converter = new AreaConverter();

        Assert.Throws<InvalidUnitException>(() =>
            converter.Convert(1, "invalid", "acres"));
        Assert.Throws<InvalidUnitException>(() =>
            converter.Convert(1, "acres", "invalid"));
    }
}
