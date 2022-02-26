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
builder.Services
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
    .AddWeixinSite(o =>
    {
        o.WebsiteToken = builder.Configuration["Weixin:WebsiteToken"];
        o.Debug = bool.Parse(builder.Configuration["Weixin:Debug"] ?? "false");
        var weixinEventSink = new DefaultWeixinEventSink();
        o.Events = new WeixinMessageEvents()
        {
            OnTextMessageReceived = ctx => weixinEventSink.OnTextMessageReceived(ctx.Sender, ctx.Args),
            OnLinkMessageReceived = ctx => weixinEventSink.OnLinkMessageReceived(ctx.Sender, ctx.Args),
            OnClickMenuEventReceived = ctx => weixinEventSink.OnClickMenuEventReceived(ctx.Sender, ctx.Args),
            OnImageMessageReceived = ctx => weixinEventSink.OnImageMessageReceived(ctx.Sender, ctx.Args),
            OnLocationEventReceived = ctx => weixinEventSink.OnLocationEventReceived(ctx.Sender, ctx.Args),
            OnLocationMessageReceived = ctx => weixinEventSink.OnLocationMessageReceived(ctx.Sender, ctx.Args),
            OnQrscanEventReceived = ctx => weixinEventSink.OnQrscanEventReceived(ctx.Sender, ctx.Args),
            OnEnterEventReceived = ctx => weixinEventSink.OnEnterEventReceived(ctx.Sender, ctx.Args),
            OnSubscribeEventReceived = ctx => weixinEventSink.OnSubscribeEventReceived(ctx.Sender, ctx.Args),
            OnUnsubscribeEventReceived = ctx => weixinEventSink.OnUnsubscribeEventReceived(ctx.Sender, ctx.Args),
            OnVideoMessageReceived = ctx => weixinEventSink.OnVideoMessageReceived(ctx.Sender, ctx.Args),
            OnShortVideoMessageReceived = ctx => weixinEventSink.OnShortVideoMessageReceived(ctx.Sender, ctx.Args),
            OnViewMenuEventReceived = ctx => weixinEventSink.OnViewMenuEventReceived(ctx.Sender, ctx.Args),
            OnVoiceMessageReceived = ctx => weixinEventSink.OnVoiceMessageReceived(ctx.Sender, ctx.Args)
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
