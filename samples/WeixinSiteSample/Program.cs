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

        //�Ƿ�����΢��web�����߹���(wechatdevtools)�ȿͻ��˷��ʣ�Ĭ��ֵΪfalse��trueΪ����
        o.Debug = bool.Parse(builder.Configuration["Weixin:Debug"] ?? "false");

        //��ע�����ֵ��ȷ����
        // ��1������д���󣬽������������á�����ģʽ���򡰰�ȫģʽ��ʱ�޷���ȷ���ܣ������ܣ���
        // ��2������ʹ�á�΢�Ź���ƽ̨���Ժš�������Ӧ��ע�⵽�䲻֧����Ϣ�ӽ��ܣ���ʱ���ÿ��ַ��������á�
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
