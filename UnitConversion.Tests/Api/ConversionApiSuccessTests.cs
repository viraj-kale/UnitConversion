using System.Net;
using System.Net.Http.Json;
using UnitConversion.Models.Requests;
using UnitConversion.Models.Responses;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UnitConversion.Tests.Api;

public class ConversionApiSuccessTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string ConversionEndpoint = "/api/v1/Conversion";

    private readonly HttpClient _client;

    public ConversionApiSuccessTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    public static TheoryData<string, string, string, double, double> ConversionCases =>
        new()
        {
            { "length", "kilometers", "miles", 1, 0.6214 },
            { "weight", "kilograms", "pounds", 1, 2.2046 },
            { "temperature", "celsius", "fahrenheit", 100, 212 },
            { "volume", "gallon", "liters", 1, 3.7854 },
            { "area", "acre", "squarefoot", 1, 43560.0573 },
            { "speed", "mileperhour", "kilometerperhour", 60, 96.5606 },
            { "time", "hours", "minutes", 2, 120 }
        };

    [Theory]
    [MemberData(nameof(ConversionCases))]
    public async Task Convert_ValidCategory_ReturnsExpectedResult(
        string category,
        string fromUnit,
        string toUnit,
        double value,
        double expected)
    {
        var response = await _client.PostAsJsonAsync(
            ConversionEndpoint,
            new ConversionRequest
            {
                Category = category,
                FromUnit = fromUnit,
                ToUnit = toUnit,
                Value = value
            });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ConversionResponse>();
        Assert.NotNull(body);
        Assert.Equal(category, body.Category);
        Assert.Equal(fromUnit, body.FromUnit);
        Assert.Equal(toUnit, body.ToUnit);
        Assert.Equal(value, body.OriginalValue);
        Assert.Equal(expected, body.ConvertedValue, precision: 2);
    }
}
