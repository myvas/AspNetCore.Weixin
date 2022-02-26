using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Myvas.AspNetCore.Weixin;
using WeixinSiteSample.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.Configure<WeixinOptions>(
    builder.Configuration.GetSection("Weixin"));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.ConfigureWarnings(b => b.Log(CoreEventId.ManyServiceProvidersCreatedWarning))
    .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
var weixinBuilder = builder.Services
    .AddWeixin(o =>
    {
      o.AppId = builder.Configuration["Weixin:AppId"];
      o.AppSecret = builder.Configuration["Weixin:AppSecret"];
    },
    o =>
    {
      o.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
      o.InstanceName = builder.Configuration["Weixin:AppId"];
    })
    .AddSubscriberManager<ApplicationDbContext>()
    .AddMessenger();
builder.Services.AddScoped<IWeixinEventSink, DefaultWeixinEventSink>();
var weixinEventSink = builder.Services.BuildServiceProvider().GetRequiredService<IWeixinEventSink>();
weixinBuilder
    .AddWeixinSite(o =>
    {
        o.WebsiteToken = builder.Configuration["Weixin:WebsiteToken"];
        o.Debug = bool.Parse(builder.Configuration["Weixin:Debug"] ?? "false");
        o.Events = new WeixinMessageEvents()
        {
            OnTextMessageReceived = ctx => weixinEventSink.OnTextMessageReceived(ctx),
            OnLinkMessageReceived = ctx => weixinEventSink.OnLinkMessageReceived(ctx),
            OnClickMenuEventReceived = ctx => weixinEventSink.OnClickMenuEventReceived(ctx),
            OnImageMessageReceived = ctx => weixinEventSink.OnImageMessageReceived(ctx),
            OnLocationEventReceived = ctx => weixinEventSink.OnLocationEventReceived(ctx),
            OnLocationMessageReceived = ctx => weixinEventSink.OnLocationMessageReceived(ctx),
            OnQrscanEventReceived = ctx => weixinEventSink.OnQrscanEventReceived(ctx),
            OnEnterEventReceived = ctx => weixinEventSink.OnEnterEventReceived(ctx),
            OnSubscribeEventReceived = ctx => weixinEventSink.OnSubscribeEventReceived(ctx),
            OnUnsubscribeEventReceived = ctx => weixinEventSink.OnUnsubscribeEventReceived(ctx),
            OnVideoMessageReceived = ctx => weixinEventSink.OnVideoMessageReceived(ctx),
            OnShortVideoMessageReceived = ctx => weixinEventSink.OnShortVideoMessageReceived(ctx),
            OnViewMenuEventReceived = ctx => weixinEventSink.OnViewMenuEventReceived(ctx),
            OnVoiceMessageReceived = ctx => weixinEventSink.OnVoiceMessageReceived(ctx)
        };
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
