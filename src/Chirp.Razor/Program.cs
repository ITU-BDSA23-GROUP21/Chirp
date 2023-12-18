using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Claims;

namespace Chirp.Razor;

public class Program {
    public static void Main(string[] args) {
        // Code architecture inspired by code example from Rasmus from lecture 6 (05.10.2023)  

        var builder = WebApplication.CreateBuilder(args);
        var connectionString = string.Empty;
        var usePostgres = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        bool uiTest = false;
        if (builder.Environment.IsDevelopment()) {
            uiTest = string.Equals(Environment.GetEnvironmentVariable("UITEST"), "1");
            if(!uiTest)
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
        if(uiTest)
        {
            string path = Path.Combine(Path.GetTempPath(), "LocalChirpTestDatabase.db");
            if(File.Exists(path))
                File.Delete(path);
            builder.Services.AddDbContext<ChirpContext>(options => options.UseSqlite($"Data Source={path}"));
        }
        else
        {
            builder.Services.AddDbContext<ChirpContext>(options => _ = usePostgres switch {
                false => options.UseSqlServer(connectionString, x => x.MigrationsAssembly("Chirp.Migrations")),
                true => options.UseNpgsql(builder.Configuration["PostgresConnectionString"], x => x.MigrationsAssembly("Chirp.PostgresMigrations"))
            });
        }
        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));
        builder.Services.AddRazorPages()
        .AddMicrosoftIdentityUI();
        builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            options.Events.OnTokenValidated = async context => {
                var identity = (ClaimsIdentity?)context?.Principal?.Identity;
                var name = identity?.Name;
                var email = identity?.Claims?.Where(c => c.Type == "emails").FirstOrDefault()?.Value;

                if (name == null || email == null || context == null) {
                    throw new AuthenticationException("Login failed");
                }

                var authorRepo = context.HttpContext.RequestServices.GetRequiredService<IAuthorRepository>();
                await authorRepo.CreateAuthor(new AuthorDto(name, email));
            }
        );

        var app = builder.Build();

        // Seed data into database.
        using (var scope = app.Services.CreateScope()) {
            var context = scope.ServiceProvider.GetRequiredService<ChirpContext>();
            // Migrating this way might be unsafe in azure environment.
            // See https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli
            if(uiTest)
                context.Database.EnsureCreated();
            else
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
