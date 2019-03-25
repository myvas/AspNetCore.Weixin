# AspNetCore.Weixin

## NuGet
https://www.nuget.org/packages/AspNetCore.Weixin/

## Settings
https://mp.weixin.qq.com

开发/基本配置/公众号开发信息
- 获取**AppSecret**

开发/基本配置/服务器配置：***修改设置*** | ***启用***
- 在“服务器地址(**URL**)"中，填写地址: http://xxx.xxx/wx or https://xxx.xxx/wx
- 在“网站**Token**”中，填写一串较长的随机字符串作为WebsiteToken
- 在“消息加解密密钥**EncodingAESKey**”中，生成一个
- 在“消息加解方式”中，**建议**选择“***安全模式***”

## ConfigureServices
```
services.AddScoped<WeixinEventSink>();
var weixinEventSink = services.BuildServiceProvider().GetRequiredService<WeixinEventSink>();
services.AddWeixin(options =>
  {
      options.AppId = _configuration["Weixin:AppId"];
      options.AppSecret = _configuration["Weixin:AppSecret"];
      options.WebsiteToken = _configuration["Weixin:WebsiteToken"];
      options.EncodingAESKey = _configuration["Weixin:EncodingAESKey"]; //请注意检查该值正确无误！（1）若填写错误，将导致您在启用“兼容模式”或“安全模式”时无法正确解密（及加密）；（2）若您使用“微信公众平台测试号”部署，您应当注意到其不支持消息加解密，此时须用空字符串或直接删除此行代码。
      
      options.Path = "/wx"; //默认值
      options.Debug = false; //默认值，不允许使用微信web开发者工具(wechatdevtools)访问。若修改为true则允许。
      
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

## Usage of IWeixinAccessToken
```
private readonly IWeixinAccessToken _weixinAccessToken;

.ctor(IWeixinAccessToken weixinAccessToken)
{
    _weixinAccessToken = weixinAccessToken;
}

public IActionResult MethodA()
{
   var token = _weixinAccessToken.GetToken();
}
```

## Demo
http://weixin.myvas.com

## Branches
- master: ASP.NET Core 2.1(LTS)
- branches:
- tags: create a tag when release to nuget

## Migrate from ASP.NET Core 2.0 to 2.1
https://docs.microsoft.com/en-us/aspnet/core/migration/20_21
