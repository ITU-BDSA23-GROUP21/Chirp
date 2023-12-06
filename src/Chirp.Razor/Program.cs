using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.Runtime.InteropServices;

namespace Chirp.Razor;

public class Program {
    public static void Main(string[] args) {
        // Code architecture inspired by code example from Rasmus from lecture 6 (05.10.2023)  

        var builder = WebApplication.CreateBuilder(args);
        var connectionString = string.Empty;
        var usePostgres = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        if (builder.Environment.IsDevelopment()) {
            connectionString = builder.Configuration["ConnectionString"];
        }
        else {
            connectionString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_AZURE_SQL_CONNECTIONSTRING");
        }

        // Add services to the container.
        builder.Services.AddScoped<ICheepService, CheepService>();
        builder.Services.AddScoped<IAuthorService, AuthorService>();
        builder.Services.AddScoped<ICheepRepository, CheepRepository>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
        builder.Services.AddDbContext<ChirpContext>(options => _ = usePostgres switch {
            false => options.UseSqlServer(connectionString, x => x.MigrationsAssembly("Chirp.Migrations")),
            true => options.UseNpgsql(builder.Configuration["PostgresConnectionString"], x => x.MigrationsAssembly("Chirp.PostgresMigrations"))
        });
        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));
        builder.Services.AddRazorPages()
        .AddMicrosoftIdentityUI();

        var app = builder.Build();

        // Seed data into database.
        using (var scope = app.Services.CreateScope()) {
            var context = scope.ServiceProvider.GetRequiredService<ChirpContext>();
            // Migrating this way might be unsafe in azure environment.
            // See https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli
            context.Database.Migrate();
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
