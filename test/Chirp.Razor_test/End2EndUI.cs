using NUnit.Framework;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;

namespace Chirp.Razor_test;

public partial class End2EndUI : PageTest
{
    private async Task LoginToGithub(IPage page)
    {
        await page.GotoAsync("https://github.com/login");
        await page.GetByLabel("Username or email address").FillAsync("dummyaccountfortesting");
        await page.GetByLabel("Password").FillAsync("dummygithubaccountfortesting");
        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();
    }

    [Test]
    public async Task BrowsePublicTimelineWenNotLoggedIn()
    {
        //Setup
        //Launches testbrowser for tracing to inspect after test
        await using var browser = await Playwright.Chromium.LaunchAsync();
        await using var context = await browser.NewContextAsync();
        //Starts tracing
        await context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
        var page = await context.NewPageAsync();
        
        //Gets sites ipaddress from environment variable 'IPADDRESS'
        string ipaddress = Environment.GetEnvironmentVariable("IPADDRESS") ?? string.Empty;
        if(string.IsNullOrEmpty(ipaddress))
            throw new NullReferenceException("The environment variable 'IPADDRESS' is null or empty and needs to be set.");

        //The playwright test
        await page.GotoAsync(ipaddress);
        var visibilityCheck = await page.GetByRole(AriaRole.Heading, new(){ Name = "Public Timeline" }).IsVisibleAsync();
        Assert.That(visibilityCheck, Is.True);
        await Expect(page.GetByRole(AriaRole.Listitem)).ToHaveCountAsync(32);
        
        //Stops tracing and saves to file
        await context.Tracing.StopAsync(new()
        {
            Path = "test1.zip"
        });
    }
    [Test]
    public async Task LoginAndBrowsePublicTimeline()
    {
        //Setup
        //Launches testbrowser for tracing to inspect after test
        await using var browser = await Playwright.Chromium.LaunchAsync();
        await using var context = await browser.NewContextAsync();
        //Starts tracing
        await context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
        var page = await context.NewPageAsync();

        //Gets sites ipaddress from environment variable 'IPADDRESS'
        string ipaddress = Environment.GetEnvironmentVariable("IPADDRESS") ?? string.Empty;
        if(string.IsNullOrEmpty(ipaddress))
            throw new NullReferenceException("The environment variable 'IPADDRESS' is null or empty and needs to be set.");

        //The playwright test
        await LoginToGithub(page);
        await page.GotoAsync(ipaddress);
        var visibilityCheck = await page.GetByRole(AriaRole.Heading, new(){ Name = "Public Timeline" }).IsVisibleAsync();
        Assert.That(visibilityCheck, Is.True);
        await Expect(page.GetByRole(AriaRole.Listitem)).ToHaveCountAsync(32);

        //Stops tracing and saves to file
        await context.Tracing.StopAsync(new()
        {
            Path = "test2.zip"
        });
    }
    [Test]
    public async Task LoginCheepFollowBrowsePrivateTimelineAndRateCheeps()
    {
        //Setup
        //Launches testbrowser for tracing to inspect after test
        await using var browser = await Playwright.Chromium.LaunchAsync();
        await using var context = await browser.NewContextAsync();
        //Starts tracing
        await context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
        var page = await context.NewPageAsync();

        //Gets sites ipaddress from environment variable 'IPADDRESS'
        string ipaddress = Environment.GetEnvironmentVariable("IPADDRESS") ?? string.Empty;
        if(string.IsNullOrEmpty(ipaddress))
            throw new NullReferenceException("The environment variable 'IPADDRESS' is null or empty and needs to be set.");

        //The playwright test
        //Login
        await LoginToGithub(page);
        await page.GotoAsync(ipaddress);
        await page.GetByRole(AriaRole.Link, new(){ Name = "login" }).ClickAsync();
        var logoutLink = page.GetByText("logout [dummyaccountfortesting]");
        await Expect(logoutLink).ToBeVisibleAsync();
        //Cheep
        var cheepBoxLabel = page.GetByRole(AriaRole.Heading, new(){ Name = "What's on your mind dummyaccountfortesting?"});
        await Expect(cheepBoxLabel).ToBeVisibleAsync();
        await page.GetByRole(AriaRole.Textbox).FillAsync("Testing the ability to cheep");
        await page.GetByText("Share", new(){ Exact = true }).ClickAsync();
        var cheep = page.GetByRole(AriaRole.Listitem).First;
        await Expect(cheep).ToHaveTextAsync(CheepRegex());
        //Follow
        await page.GetByRole(AriaRole.Listitem).Nth(1).GetByRole(AriaRole.Button, new(){ Name = "[Follow]" }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new(){ Name = "my timeline "}).ClickAsync();
        await Expect(page.GetByRole(AriaRole.Heading, new(){ Level = 2 }).First).ToHaveTextAsync("dummyaccountfortesting's Timeline");
        var mycheep = page.GetByRole(AriaRole.Listitem).First;
        await Expect(mycheep).ToHaveTextAsync(CheepRegex());
        var followingCheep1 = page.GetByRole(AriaRole.Listitem).Nth(1);
        await Expect(followingCheep1.GetByRole(AriaRole.Link).First).ToHaveTextAsync("Jacqualine Gilcoine");
        //Like/Dislike
        await Expect(followingCheep1).ToHaveTextAsync(ZeroCountRegex());
        await followingCheep1.GetByRole(AriaRole.Button, new(){ Name = "Like", Exact = true }).ClickAsync();
        await Expect(followingCheep1).ToHaveTextAsync(OneCountRegex());
        var followingCheep2 = page.GetByRole(AriaRole.Listitem).Nth(2);
        await Expect(followingCheep2).ToHaveTextAsync(ZeroCountRegex());
        await followingCheep2.GetByRole(AriaRole.Button, new(){ Name = "Dislike", Exact = true }).ClickAsync();
        await Expect(followingCheep2).ToHaveTextAsync(MinusOneCountRegex());

        //Stops tracing and saves to file
        await context.Tracing.StopAsync(new()
        {
            Path = "test3.zip"
        });
    }
    [Test]
    public async Task LoginCheepLookAtUserInformationAndForgetUser()
    {
        //Setup
        //Launches testbrowser for tracing to inspect after test
        await using var browser = await Playwright.Chromium.LaunchAsync();
        await using var context = await browser.NewContextAsync();
        //Starts tracing
        await context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
        var page = await context.NewPageAsync();

        //Gets sites ipaddress from environment variable 'IPADDRESS'
        string ipaddress = Environment.GetEnvironmentVariable("IPADDRESS") ?? string.Empty;
        if(string.IsNullOrEmpty(ipaddress))
            throw new NullReferenceException("The environment variable 'IPADDRESS' is null or empty and needs to be set.");

        //The playwright test
        await LoginToGithub(page);
        await page.GotoAsync(ipaddress);
        await page.GetByRole(AriaRole.Link, new(){ Name = "login" }).ClickAsync();
        var cheepbox = page.GetByRole(AriaRole.Textbox).First;
        await Expect(cheepbox).ToBeVisibleAsync();
        await cheepbox.FillAsync("Test Cheep");
        await page.GetByText("Share", new(){ Exact = true }).ClickAsync();
        var myInfoLink = page.GetByRole(AriaRole.Link, new(){ Name = "My Information" });
        await Expect(myInfoLink).ToBeVisibleAsync();
        await myInfoLink.ClickAsync();
        var nameInfo = page.GetByText("Name: ");
        await Expect(nameInfo).ToContainTextAsync("dummyaccountfortesting");
        var emailInfo = page.GetByText("Email: ");
        await Expect(emailInfo).ToContainTextAsync("dummyemailfortesting000@gmail.com");
        await page.GetByRole(AriaRole.Button, new(){ Name = "Delete account"}).ClickAsync();
        var signedOutHeader = page.GetByRole(AriaRole.Heading, new(){ Name = "Signed out" });
        await Expect(signedOutHeader).ToBeVisibleAsync();
        await page.GetByRole(AriaRole.Link, new(){ Name = "public timeline" }).ClickAsync();
        var cheep = page.GetByRole(AriaRole.Listitem).First;
        await Expect(cheep).Not.ToContainTextAsync("dummyaccountfortesting");

        //Stops tracing and saves to file
        await context.Tracing.StopAsync(new()
        {
            Path = "test4.zip"
        });
    }

    [GeneratedRegex("[\\s\\S]*Testing the ability to cheep[\\s\\S]*")]
    private static partial Regex CheepRegex();
    [GeneratedRegex("Like[\\s]*0[\\s]*Dislike")]
    private static partial Regex ZeroCountRegex();
    [GeneratedRegex("Like[\\s]*1[\\s]*Dislike")]
    private static partial Regex OneCountRegex();
    [GeneratedRegex("Like[\\s]*-1[\\s]*Dislike")]
    private static partial Regex MinusOneCountRegex();
}