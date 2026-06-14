using System.ComponentModel.DataAnnotations;
using UnitConversion.Constants;

namespace UnitConversion.Models.Requests;

/// <summary>
/// Request body for converting a value between two units within the same category.
/// </summary>
public class ConversionRequest
{
    /// <summary>
    /// Conversion category. Supported values: length, weight, temperature, volume, area, speed, time.
    /// </summary>
    /// <example>length</example>
    [Required(ErrorMessage = "Category is required.")]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Source unit to convert from. Must match the selected category.
    /// </summary>
    /// <example>kilometers</example>
    [Required(ErrorMessage = "FromUnit is required.")]
    public string FromUnit { get; set; } = string.Empty;

    /// <summary>
    /// Target unit to convert to. Must match the selected category.
    /// </summary>
    /// <example>miles</example>
    [Required(ErrorMessage = "ToUnit is required.")]
    public string ToUnit { get; set; } = string.Empty;

    /// <summary>
    /// Numeric value to convert.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Value is required.")]
    public double? Value { get; set; }
}
