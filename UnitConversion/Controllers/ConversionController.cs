using Microsoft.AspNetCore.Mvc;
using UnitConversion.Models.Requests;
using UnitConversion.Models.Responses;

namespace UnitConversion.Controllers;

/// <summary>
/// Converts values between units for length, weight, temperature, volume, area, speed, and time.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ConversionController : ControllerBase
{
    private readonly IConversionService _service;

    public ConversionController(
        IConversionService service)
    {
        _service = service;
    }

    /// <summary>
    /// Convert a value from one unit to another within a supported category.
    /// </summary>
    /// <remarks>
    /// **length** — meter, kilometer, foot, mile
    ///
    /// **weight** — gram, kilogram, pound
    ///
    /// **temperature** — celsius, fahrenheit, kelvin
    ///
    /// **volume** — liter, milliliter, gallon, cup
    ///
    /// **area** — squaremeter, squarefoot, acre, hectare
    ///
    /// **speed** — meterpersecond, kilometerperhour, mileperhour
    ///
    /// **time** — second, minute, hour, day
    ///
    /// Unit names are case-insensitive. Most categories accept singular and plural forms.
    /// </remarks>
    /// <param name="request">The conversion request containing category, units, and value.</param>
    /// <returns>The converted value and original request details.</returns>
    /// <response code="200">Conversion completed successfully.</response>
    /// <response code="400">Validation failed, unsupported category, or invalid unit.</response>
    /// <response code="500">Unexpected server error.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ConversionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult Convert(
        [FromBody] ConversionRequest request)
    {
        var result = _service.Convert(request);

        return Ok(result);
    }
}
