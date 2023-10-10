using Microsoft.AspNetCore.Mvc.Testing;
using Chirp.Razor;

namespace Chirp.Razor_test;

[Collection("Environment Variable")]
public class End2End : IClassFixture<WebApplicationFactory<Program>> {
    private readonly WebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;

    public End2End(WebApplicationFactory<Program> fixture) {
        _fixture = fixture;
        _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
    }

    //Test case taking from lecture slides
    [Fact]
    public async void CanSeePublicTimeline() {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains("Public Timeline", content);
    }

    [Theory]
    [InlineData("Octavio Wagganer")]
    [InlineData("Quintin Sitts")]
    public async void CanSeePrivateTimeline(string author) {
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains($"{author}'s Timeline", content);
    }
}