using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WeixinSiteSample.Data;

namespace WeixinSiteSample.Data;

public static class WebApplicationDatabaseExtensions
{
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var db = services.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }

        return app;
    }


    public static WebApplication SeedDatabase(this WebApplication app, string adminUserName, string adminInitPassword)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                Task.Run(async () =>
                {
                    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                    await EnsureAdminUser(userManager, adminUserName, adminInitPassword);
                });
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        return app;
    }

    private static async Task EnsureAdminUser(UserManager<IdentityUser> userManager, string adminUserName, string adminEmail)
    {
        var adminInitPassword = adminEmail;

        var user = await userManager.FindByNameAsync(adminUserName);
        if (user == null)
        {
            user = new IdentityUser()
            {
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, adminInitPassword);
            if (!result.Succeeded)
            {
                throw new Exception(GetErrorMessage(result));
            }
        }
    }

    private static string GetErrorMessage(IdentityResult identityResult)
    {
        var result = "";

        foreach (var error in identityResult.Errors)
        {
            result += $"[{error.Code}]{error.Description}" + Environment.NewLine;
        }
        return result;
    }
}