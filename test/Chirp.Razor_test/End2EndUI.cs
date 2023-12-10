using Microsoft.Data.SqlClient;
using NUnit.Framework;
using Testcontainers.MsSql;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Diagnostics;

namespace Chirp.Razor_test;

public class End2EndUI : PageTest
{
    public async Task LoginToGithub(IPage page)
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
        await page.GetByRole(AriaRole.Heading, new(){ Name = "Public Timeline" }).IsVisibleAsync();
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
        await page.GetByRole(AriaRole.Heading, new(){ Name = "Public Timeline" }).IsVisibleAsync();
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
        await page.GetByText("logout [dummyaccountfortesting]").IsVisibleAsync();
        //Cheep
        await page.GetByRole(AriaRole.Heading, new(){ Name = "What's on your mind dummyaccountfortesting?"}).IsVisibleAsync();
        await page.GetByRole(AriaRole.Textbox, new(){ Name = "NewMessage"}).FillAsync("Testing the ability to cheep");
        await page.GetByText("Share", new(){ Exact = true }).ClearAsync();
        var cheep = page.GetByRole(AriaRole.Listitem).First;
        await Expect(cheep).ToHaveTextAsync("Testing the ability to cheep");
        //Follow
        await page.GetByRole(AriaRole.Listitem).Nth(1).GetByRole(AriaRole.Button, new(){ Name = "[Follow]" }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new(){ Name = "my timeline "}).ClickAsync();
        var mycheep = page.GetByRole(AriaRole.Listitem).First;
        await Expect(mycheep).ToHaveTextAsync("Testing the ability to cheep");
        var cheepAuthor = page.GetByRole(AriaRole.Listitem).Nth(1).GetByRole(AriaRole.Link).First;
        await Expect(cheepAuthor).ToHaveTextAsync("Jacqualine Gilcoine");
        



        //Stops tracing and saves to file
        await context.Tracing.StopAsync(new()
        {
            Path = "test2.zip"
        });
    }
    [Test]
    public async Task LoginLookAtUserInformationAndForgetUser()
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


        //Stops tracing and saves to file
        await context.Tracing.StopAsync(new()
        {
            Path = "test2.zip"
        });
    }
}