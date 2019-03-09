# AspNetCore.Weixin

## Migrate from ASP.NET Core 2.0 to 2.1
https://docs.microsoft.com/en-us/aspnet/core/migration/20_21

## NuGet
https://www.nuget.org/packages/AspNetCore.Weixin/

## Settings
https://mp.weixin.qq.com

- 获取AppSecret
- 启用开发模式，部署网站并启用https/ssl，网址类似: https://xxx.xxx/wx
- 在“网站Token”中填写一串较长的随机字符串作为WebsiteToken
- 不要选择“加密模式”（本版本暂不支持）


## ConfigureServices
```
services.AddScoped<WeixinEventSink>();
var weixinEventSink = services.BuildServiceProvider().GetRequiredService<WeixinEventSink>();
services.AddWeixin(options =>
  {
      options.AppId = _configuration["Weixin:AppId"];
      options.AppSecret = _configuration["Weixin:AppSecret"];
      options.WebsiteToken = _configuration["Weixin:WebsiteToken"];
      options.EncodingAESKey = _configuration["Weixin:EncodingAESKey"];
      options.Events = new WeixinMessageEvents()
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
```

## Configure
```
app.UseWeixinWelcomePage();
```

## Demo
http://weixin.myvas.com

## 分支及规划
- master分支：工作在ASP.NET Core的LTS版本，当前LTS版本为2.1。本程序现工作在SDK 2.1.504上。
- tag与发布：每次产生一个新的打包并正式发布到nuget上时，创建一个tag
