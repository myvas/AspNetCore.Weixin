using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin;
using WeixinSiteSample.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Settings in one of following
// (1) appsettings.json or appsettings.<EnvironmentName>.json
// (2) ~/.microsoft/usersecrets/WeixinSiteMvcSample/secrets.json
// (3) environment varibles (replace each colon with double-underscores, eg. WEIXIN__APPID)
var Configuration = builder.Configuration;
var Services = builder.Services;
Services.AddDbContext<AppDbContext>(o =>
{
    o.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
});
Services.AddWeixin(o =>
{
    o.AppId = Configuration["Weixin:AppId"];
    o.AppSecret = Configuration["Weixin:AppSecret"];
})
.AddAccessTokenRedisCacheProvider(o =>
{
    // See: https://stackexchange.github.io/StackExchange.Redis/Configuration.html
    o.Configuration = Configuration.GetValue("Weixin:RedisConnection", "localhost");
})
.AddWeixinSite(o =>
{
    o.Path = Configuration["Weixin:Path"]; // Default: "/wx"
    o.WebsiteToken = Configuration["Weixin:WebsiteToken"];
})
.AddMessageProtection(o =>
{
    o.StrictMode = Configuration.GetValue<bool>("Weixin:StrictMode", false); // Default: false
    o.EncodingAESKey = Configuration["Weixin:EncodingAESKey"];
})
.AddWeixinEfCore<AppDbContext>(o =>
{
    o.EnableSyncForWeixinSubscribers = Configuration.GetValue<bool>("Weixin:EnableSync", false); // Default: false
    o.SyncIntervalInMinutesForWeixinSubscribers = Configuration.GetValue<int>("Weixin:SyncIntervalInMinutes", 60 * 24); // Set Default: 1 day
});

var app = builder.Build();

// Migrate the database, and seed the admin user and email.
// (The initial password is exactly this email.)
var adminUserName = Configuration.GetValue("Weixin:AdminUserName", "demo"); // Default: demo
var adminEmail = Configuration.GetValue("Weixin:AdminEmail", "demo@myvas.com"); // Default: demo@myvas.com
app.MigrateDatabase().SeedDatabase(adminUserName, adminEmail);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// Use the WeixinSiteMiddleware.
app.UseWeixinSite();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
