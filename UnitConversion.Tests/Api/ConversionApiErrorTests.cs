using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using UnitConversion.Models.Requests;

namespace UnitConversion.Tests.Api;

public class ConversionApiErrorTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string ConversionEndpoint = "/api/v1/Conversion";

    private readonly HttpClient _client;

    public ConversionApiErrorTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Convert_MissingFields_ReturnsValidationProblemDetails()
    {
        var response = await _client.PostAsJsonAsync(ConversionEndpoint, new { });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        using var document = await JsonDocument.ParseAsync(
            await response.Content.ReadAsStreamAsync());

        Assert.Equal("VALIDATION_ERROR", document.RootElement.GetProperty("errorCode").GetString());
        Assert.Equal("Validation failed", document.RootElement.GetProperty("title").GetString());
        Assert.True(document.RootElement.TryGetProperty("errors", out _));
    }

    [Fact]
    public async Task Convert_UnsupportedCategory_ReturnsProblemDetails()
    {
        var response = await _client.PostAsJsonAsync(
            ConversionEndpoint,
            new ConversionRequest
            {
                Category = "distance",
                FromUnit = "meters",
                ToUnit = "feet",
                Value = 1
            });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        using var document = await JsonDocument.ParseAsync(
            await response.Content.ReadAsStreamAsync());

        Assert.Equal("UNSUPPORTED_CATEGORY", document.RootElement.GetProperty("errorCode").GetString());
        Assert.Contains(
            "not supported",
            document.RootElement.GetProperty("detail").GetString(),
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Convert_InvalidUnit_ReturnsProblemDetails()
    {
        var response = await _client.PostAsJsonAsync(
            ConversionEndpoint,
            new ConversionRequest
            {
                Category = "length",
                FromUnit = "yards",
                ToUnit = "meters",
                Value = 1
            });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        using var document = await JsonDocument.ParseAsync(
            await response.Content.ReadAsStreamAsync());

        Assert.Equal("INVALID_UNIT", document.RootElement.GetProperty("errorCode").GetString());
        Assert.Equal("fromUnit", document.RootElement.GetProperty("unitRole").GetString());
    }

    [Fact]
    public async Task Convert_ValidRequest_ReturnsOk()
    {
        var response = await _client.PostAsJsonAsync(
            ConversionEndpoint,
            new ConversionRequest
            {
                Category = "length",
                FromUnit = "kilometers",
                ToUnit = "miles",
                Value = 1
            });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Convert_InvalidJson_ReturnsBadRequest()
    {
        var content = new StringContent("{ invalid json", System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(ConversionEndpoint, content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
