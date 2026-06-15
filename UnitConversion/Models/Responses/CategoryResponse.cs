namespace UnitConversion.Models.Responses;

/// <summary>
/// A supported conversion category and its units.
/// </summary>
public class CategoryResponse
{
    /// <summary>
    /// Category identifier used in conversion requests.
    /// </summary>
    /// <example>length</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable description of the category.
    /// </summary>
    /// <example>Distance and dimension measurements.</example>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Canonical unit names supported for this category.
    /// </summary>
    public IReadOnlyList<string> Units { get; set; } = [];
}
