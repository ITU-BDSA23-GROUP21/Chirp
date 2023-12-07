using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Chirp.Razor_test;

public class Tests : PageTest
{
    private string ipaddress;

    [OneTimeSetUp]
    public void OnetimeSetup()
    {
        ipaddress = Environment.GetEnvironmentVariable("IPADDRESS") ?? string.Empty;
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        await Page.GotoAsync(ipaddress);
        
        var publicTimeline = Page.GetByRole(Microsoft.Playwright.AriaRole.Link, new(){ Name = "public timeline"});
        await Expect(publicTimeline).ToHaveAttributeAsync("href", "/");
    }
}