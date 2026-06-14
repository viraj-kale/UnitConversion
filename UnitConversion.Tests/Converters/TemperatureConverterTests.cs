using UnitConversion.Tests.Converters;

namespace UnitConversion.Tests.Converters;

public class TemperatureConverterTests
{
    [Fact]
    public void Convert_Celsius_To_Fahrenheit_Should_Return_Correct_Value()
    {
        // Arrange
        var converter = new TemperatureConverter();
        // Act
        var result = converter.Convert(
            0,
            "celsius",
            "fahrenheit");
        // Assert
        Assert.Equal(32, result);
    }
    [Fact]
    public void Convert_Fahrenheit_To_Celsius_Should_Return_Correct_Value()
    {
        // Arrange
        var converter = new TemperatureConverter();
        // Act
        var result = converter.Convert(
            32,
            "fahrenheit",
            "celsius");
        // Assert
        Assert.Equal(0, result);
    }
    [Fact]
    public void Convert_Celsius_To_Kelvin_Should_Return_Correct_Value()
    {
        // Arrange
        var converter = new TemperatureConverter();
        // Act
        var result = converter.Convert(
            0,
            "celsius",
            "kelvin");
        // Assert
        Assert.Equal(273.15, result);
    }
     [Fact]
     public void Convert_Kelvin_To_Celsius_Should_Return_Correct_Value()
    {
        // Arrange
        var converter = new TemperatureConverter();
        // Act
        var result = converter.Convert(
            273.15,
            "kelvin",
            "celsius");
        // Assert
        Assert.Equal(0, result);
    }
     [Fact]
     public void Convert_Invalid_Unit_Should_Throw_Exception()
    {
        // Arrange
        var converter = new TemperatureConverter();
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            converter.Convert(0, "invalid", "celsius"));
    }
}