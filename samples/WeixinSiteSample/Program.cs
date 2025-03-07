using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Myvas.AspNetCore.Weixin;
using WeixinSiteSample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.ConfigureWarnings(b => b.Log(CoreEventId.ManyServiceProvidersCreatedWarning))
    .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services
    //IWeixinApi: utilities to communicate with the tencent server.
    .AddWeixin(o =>
    {
        o.AppId = builder.Configuration["Weixin:AppId"];
        o.AppSecret = builder.Configuration["Weixin:AppSecret"];
    })
    //IWeixinAccessToken: get access_token cached in redis, or fetch it from the remote.
    .AddAccessToken(o =>
    {
        o.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
        o.InstanceName = builder.Configuration["Weixin:AppId"];
    })
    //Weixin site handlers
    .AddWeixinSite<DefaultWeixinEventSink, ApplicationUser, ApplicationDbContext>(o =>
    {
        o.WebsiteToken = builder.Configuration["Weixin:WebsiteToken"];

        //是否允许微信web开发者工具(wechatdevtools)等客户端访问？默认值为false，true为允许。
        o.Debug = bool.Parse(builder.Configuration["Weixin:Debug"] ?? "false");

        //请注意检查该值正确无误！
        // （1）若填写错误，将导致您在启用“兼容模式”或“安全模式”时无法正确解密（及加密）；
        // （2）若您使用“微信公众平台测试号”部署，您应当注意到其不支持消息加解密，此时须用空字符串或不配置。
        //o.EncodingAESKey = builder.Configuration["Weixin:EncodingAESKey"];
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseWeixinSite();

app.Run();
