using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace Chirp.Razor;

public class Program {
    public static void Main(string[] args) {
        // Code architecture inspired by code example from Rasmus from lecture 6 (05.10.2023)  

        var builder = WebApplication.CreateBuilder(args);
        var connectionString = string.Empty;
        if (builder.Environment.IsDevelopment()) {
            var conStrBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));
            conStrBuilder.Password = builder.Configuration["DatabasePassword"];
            connectionString = conStrBuilder.ConnectionString;
        }
        else {
            connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
        }
       
        // Add services to the container.
        builder.Services.AddScoped<ICheepService, CheepService>();
        builder.Services.AddScoped<ICheepRepository, CheepRepository>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
        builder.Services.AddDbContext<ChirpContext>(Options => Options.UseSqlServer(connectionString));

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));
        builder.Services.AddRazorPages()
        .AddMicrosoftIdentityUI();

        var app = builder.Build();

        // Seed data into database.
        using (var scope = app.Services.CreateScope()) {
            var context = scope.ServiceProvider.GetRequiredService<ChirpContext>();
            context.Database.EnsureCreated();
            DbInitializer.SeedDatabase(context);
        }


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else {
            app.UseCookiePolicy(new CookiePolicyOptions() {
                MinimumSameSitePolicy = SameSiteMode.None,
                Secure = CookieSecurePolicy.Always
            });
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();

        app.Run();
    }
}
