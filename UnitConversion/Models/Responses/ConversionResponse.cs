namespace UnitConversion.Models.Responses;

/// <summary>
/// Result of a unit conversion.
/// </summary>
public class ConversionResponse
{
    /// <summary>
    /// The conversion category used for this request.
    /// </summary>
    /// <example>length</example>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// The original input value before conversion.
    /// </summary>
    /// <example>1</example>
    public double OriginalValue { get; set; }

    /// <summary>
    /// The source unit that was converted from.
    /// </summary>
    /// <example>kilometers</example>
    public string FromUnit { get; set; } = string.Empty;

    /// <summary>
    /// The target unit that was converted to.
    /// </summary>
    /// <example>miles</example>
    public string ToUnit { get; set; } = string.Empty;

    /// <summary>
    /// The converted result, rounded to four decimal places.
    /// </summary>
    /// <example>0.6214</example>
    public double ConvertedValue { get; set; }
}
