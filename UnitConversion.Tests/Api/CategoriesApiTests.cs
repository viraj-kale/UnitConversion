using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace UnitConversion.Tests.Api;

public class CategoriesApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CategoriesApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCategories_ReturnsAllSupportedCategories()
    {
        var response = await _client.GetAsync("/api/v1/categories");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = await JsonDocument.ParseAsync(
            await response.Content.ReadAsStreamAsync());

        Assert.Equal(7, document.RootElement.GetArrayLength());

        var names = document.RootElement.EnumerateArray()
            .Select(item => item.GetProperty("name").GetString())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        Assert.Contains("length", names);
        Assert.Contains("weight", names);
        Assert.Contains("temperature", names);
        Assert.Contains("volume", names);
        Assert.Contains("area", names);
        Assert.Contains("speed", names);
        Assert.Contains("time", names);
    }
}
