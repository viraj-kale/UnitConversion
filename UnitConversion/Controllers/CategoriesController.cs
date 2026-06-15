using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using UnitConversion.Constants;
using UnitConversion.Models.Responses;

namespace UnitConversion.Controllers;

/// <summary>
/// Lists supported conversion categories and units.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    /// <summary>
    /// Get all supported conversion categories and their units.
    /// </summary>
    /// <returns>Supported categories with unit lists.</returns>
    /// <response code="200">Categories returned successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CategoryResponse>), StatusCodes.Status200OK)]
    public IActionResult GetCategories()
    {
        var categories = SupportedUnits.All
            .Select(category => new CategoryResponse
            {
                Name = category.Name,
                Description = category.Description,
                Units = category.Units
            })
            .ToArray();

        return Ok(categories);
    }
}
