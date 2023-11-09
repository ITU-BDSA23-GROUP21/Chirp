using Microsoft.AspNetCore.Mvc.Testing;
using Chirp.Razor;
using Testcontainers.MsSql;

namespace Chirp.Razor_test;

[Collection("Environment Variable")]
public class End2End : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime {
    private readonly MsSqlContainer _sqlContainer;
    private WebApplicationFactory<Program> _fixture;

    public End2End(WebApplicationFactory<Program> fixture) {
        _sqlContainer = new MsSqlBuilder().Build();
        _fixture = fixture;
    }

    // Test case taking from lecture slides
    [Fact]
    public async Task CanSeePublicTimeline() {
        HttpClient _client = InitializeClient();
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains("Public Timeline", content);
    }

    [Theory]
    [InlineData("Octavio Wagganer")]
    [InlineData("Quintin Sitts")]
    public async Task CanSeePrivateTimeline(string author) {
        HttpClient _client = InitializeClient();
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains($"{author}'s Timeline", content);
    }

    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();
        Console.WriteLine(_sqlContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("TEST_CONNECTIONSTRING", _sqlContainer.GetConnectionString());
    }

    public HttpClient InitializeClient()
    {
        return _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
    }

    public Task DisposeAsync()
        => _sqlContainer.DisposeAsync().AsTask();
}