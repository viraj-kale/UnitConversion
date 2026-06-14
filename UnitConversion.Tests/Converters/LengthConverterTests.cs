using UnitConversion.Tests.Converters;

namespace UnitConversion.Tests.Converters;

public class LengthConverterTests
{
    [Fact]
    public void Convert_Meters_To_Feet_Should_Return_Correct_Value()
    {
        // Arrange
        var converter = new LengthConverter();
        // Act
        var result = converter.Convert(
            1,
            "meters",
            "feet");
        // Assert
        Assert.Equal(3.28084, result, 5);
    }
    [Fact]
    public void Convert_Feet_To_Meters_Should_Return_Correct_Value()
    {
        // Arrange
        var converter = new LengthConverter();
        // Act
        var result = converter.Convert(
            3.28084,
            "feet",
            "meters");
        // Assert
        Assert.Equal(1, result, 5);
    }
    [Fact]
    public void Convert_Kilometers_To_Miles_Should_Return_Correct_Value()
    {
        // Arrange
        var converter = new LengthConverter();
        // Act
        var result = converter.Convert(
            1,
            "kilometers",
            "miles");
        // Assert
        Assert.Equal(0.621371, result, 6);
    }
     [Fact]
     public void Convert_Miles_To_Kilometers_Should_Return_Correct_Value()
        {
            // Arrange
            var converter = new LengthConverter();
            // Act
            var result = converter.Convert(
                0.621371,
                "miles",
                "kilometers");
            // Assert
            Assert.Equal(1, result, 6);
    }
    [Fact]
    public void Convert_Invalid_Unit_Should_Throw_Exception()
    {
        // Arrange
        var converter = new LengthConverter();
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            converter.Convert(1, "invalid", "meters"));
        Assert.Throws<InvalidOperationException>(() =>
            converter.Convert(1, "meters", "invalid"));
    }
}