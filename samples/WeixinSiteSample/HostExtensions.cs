using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Authentication;
using Myvas.AspNetCore.Weixin;
using WeixinSiteSample.Data;

namespace WeixinSiteSample;

public static class HostExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        Debug.WriteLine("Create a temporary logger for me.");
        // Build a temporary logger for me.
        using var loggerFactory = LoggerFactory.Create(logging =>
        {
            logging.AddConsole();
#if DEBUG
            logging.SetMinimumLevel(LogLevel.Trace);
#else
            logging.SetMinimumLevel(LogLevel.Information);
#endif
        });
        var logger = loggerFactory.CreateLogger<WebApplicationBuilder>();
        logger.LogDebug($"{MethodBase.GetCurrentMethod()?.Name}...");
        logger.LogInformation("Environment=" + builder.Environment.EnvironmentName);

        // Settings in one of following
        // (1) appsettings.json or appsettings.<EnvironmentName>.json
        // (2) ~/.microsoft/usersecrets/WeixinSiteMvcSample/secrets.json
        // (3) environment varibles (replace each colon with double-underscores, eg. WEIXIN__APPID)
        var Configuration = builder.Configuration;
        var Services = builder.Services;

        Services.AddControllersWithViews()
#if DEBUG
            .AddRazorRuntimeCompilation()
#endif
        ;

        // Add ViewDivert        
        builder.Services.AddViewDivert();

        Services.AddDbContext<AppDbContext>(o =>
        {
            //o.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            o.UseSqlite(Configuration.GetValue("ConnectionStrings:DefaultConnection", "Data Source=app.db"));
        });

        // Add Identity 
        Services.AddDefaultIdentity<IdentityUser>()
            .AddDefaultUI()
            .AddEntityFrameworkStores<AppDbContext>();
        Services.Configure<IdentityOptions>(o =>
        {
            o.Password = new PasswordOptions
            {
                RequireLowercase = false,
                RequireUppercase = false,
                RequireNonAlphanumeric = false,
                RequireDigit = false
            };
            o.User.RequireUniqueEmail = true;
            o.SignIn.RequireConfirmedEmail = true;
            o.SignIn.RequireConfirmedPhoneNumber = false;
        });
        Services.ConfigureApplicationCookie(o =>
        {
            o.LoginPath = "/Identity/Account/Login";
            o.LogoutPath = "/Identity/Account/Logout";
            o.AccessDeniedPath = "/Identity/Account/AccessDenied";
        });

        // Add Third-party middlewares

        logger?.LogInformation("WeixinOpen:AppId=" + Configuration["WeixinOpen:AppId"]);
        logger?.LogInformation("WeixinAuth:AppId=" + Configuration["WeixinAuth:AppId"]);
        logger?.LogInformation("QQConnect:AppId=" + Configuration["QQConnect:AppId"]);

        Services.AddAuthentication()
        .AddWeixinOpen(o =>
        {
            o.AppId = Configuration["WeixinOpen:AppId"];
            o.AppSecret = Configuration["WeixinOpen:AppSecret"];
            o.SaveTokens = true;
        })
        .AddWeixinAuth(o =>
        {
            o.AppId = Configuration["WeixinAuth:AppId"];
            o.AppSecret = Configuration["WeixinAuth:AppSecret"];
            o.SilentMode = false; //不采用静默模式
            //options.SaveTokens = true;
        })
        .AddQQConnect(o =>
        {
            o.AppId = Configuration["QQConnect:AppId"];
            o.AppKey = Configuration["QQConnect:AppKey"];
            //options.SaveTokens = true;
            QQConnectScopes.TryAdd(o.Scope,
                QQConnectScopes.get_user_info,
                QQConnectScopes.list_album,
                QQConnectScopes.upload_pic,
                QQConnectScopes.do_like);
        });

        // Add IEmailSender
        logger?.LogInformation("Email:SenderAccount" + Configuration["Email:SenderAccount"]);
        Services.AddEmail(o =>
        {
            o.SmtpServerAddress = Configuration["Email:SmtpServerAddress"];
            o.SenderAccount = Configuration["Email:SenderAccount"];
            o.SenderPassword = Configuration["Email:SenderPassword"];
            o.SenderDisplayName = Configuration["Email:SenderDisplayName"];
        });

        // Add ISmsSender
        logger?.LogInformation("TencensSms:SdkAppId=" + Configuration["TencentSms:SdkAppId"]);

        Services.AddTencentSms(o =>
        {
            o.SdkAppId = Configuration["TencentSms:SdkAppId"];
            o.AppKey = Configuration["TencentSms:AppKey"];
        });

        logger?.LogInformation("Weixin:AppId=" + Configuration["Weixin:AppId"]);

        Services.AddWeixin(o =>
        {
            o.AppId = Configuration["Weixin:AppId"];
            o.AppSecret = Configuration["Weixin:AppSecret"];
        })
        .AddAccessTokenRedisCacheProvider(o =>
        {
            // See: https://stackexchange.github.io/StackExchange.Redis/Configuration.html
            o.Configuration = Configuration.GetValue("ConnectionStrings:RedisConnection", "localhost");
        })
        .AddJsapiTicketRedisCacheProvider()
        .AddWeixinSite(o =>
        {
            o.Debug = Configuration.GetValue<bool>("Weixin:Debug", true); // for this demo for debugging
            //o.Path = "/wx"; // It's the default
            o.WebsiteToken = Configuration["Weixin:WebsiteToken"];
        })
        .AddMessageProtection(o =>
        {
            o.EncodingAESKey = Configuration["Weixin:EncodingAESKey"];
            o.StrictMode = Configuration.GetValue<bool>("Weixin:StrictMode", false);
        })
        .AddWeixinEfCore<AppDbContext>(o =>
        {
            o.EnableSyncForWeixinSubscribers = Configuration.GetValue<bool>("Weixin:EnableSync", true);
            o.SyncIntervalInMinutesForWeixinSubscribers = Configuration.GetValue<int>("Weixin:SyncIntervalInMinutes", 60);
        })
        .AddWeixinEventSink<WeixinSiteSample.WeixinEventSink>();

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        var logger = app.Logger;
        logger.LogTrace($"{MethodBase.GetCurrentMethod()?.Name}...");
        logger.LogInformation($"Environment={app.Environment.EnvironmentName}");

        // Migrate the database, and seed the admin user and email.
        // (The initial password is exactly this email.)
        var adminUserName = app.Configuration.GetValue("App:AdminUserName", "demo") ?? "demo"; // Default: demo
        var adminEmail = app.Configuration.GetValue("App:AdminEmail", "demo@myvas.com") ?? "demo@myvas.com"; // Default: demo@myvas.com
        app.MigrateDatabase().SeedDatabase(adminUserName, adminEmail);

        var env = app.Environment;
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //app.UseHsts();
        }
        //app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        // Use the WeixinSiteMiddleware.
        logger.LogTrace("UseWeixinSite...");
        app.UseWeixinSite();

        // Map area route for "Identity"
        app.MapAreaControllerRoute(
            name: "Identity",
            areaName: "Identity",
            pattern: "Identity/{controller=Home}/{action=Index}/{id?}");

        // Map default controller route
        app.MapDefaultControllerRoute();

        // Map Razor Pages
        app.MapRazorPages();

        return app;
    }
}
