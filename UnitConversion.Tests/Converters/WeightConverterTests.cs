using UnitConversion.Converters;
using UnitConversion.Exceptions;

namespace UnitConversion.Tests.Converters;

public class WeightConverterTests
{
    [Fact]
    public void Convert_Kilograms_To_Pounds_Should_Return_Correct_Value()
    {
        // Arrange
        var converter = new WeightConverter();
        // Act
        var result = converter.Convert(
            1,
            "kilograms",
            "pounds");
        // Assert
        Assert.Equal(2.20462, result, 5);
    }
    [Fact]
    public void Convert_Pounds_To_Kilograms_Should_Return_Correct_Value()
    {
        // Arrange
        var converter = new WeightConverter();
        // Act
        var result = converter.Convert(
            2.20462,
            "pounds",
            "kilograms");
        // Assert
        Assert.Equal(1, result, 5);
    }
     [Fact]
     public void Convert_Grams_To_Kilograms_Should_Return_Correct_Value()
    {
        // Arrange
        var converter = new WeightConverter();
        // Act
        var result = converter.Convert(
            1000,
            "grams",
            "kilograms");
        // Assert
        Assert.Equal(1, result, 5);
    }
     [Fact]
     public void Convert_Kilograms_To_Grams_Should_Return_Correct_Value()
    {
        // Arrange
        var converter = new WeightConverter();
        // Act
        var result = converter.Convert(
            1,
            "kilograms",
            "grams");
        // Assert
        Assert.Equal(1000, result, 5);
    }
     [Fact]
     public void Convert_Invalid_Unit_Should_Throw_Exception()
     {
         // Arrange
         var converter = new WeightConverter();
         // Act & Assert
         Assert.Throws<InvalidUnitException>(() =>
             converter.Convert(1, "invalid", "grams"));
         Assert.Throws<InvalidUnitException>(() =>
             converter.Convert(1, "grams", "invalid"));
    }

}