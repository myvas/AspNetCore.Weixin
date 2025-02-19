
[![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/myvas/AspNetCore.Weixin?label=github)](https://github.com/myvas/AspNetCore.Weixin)
[![test](https://github.com/myvas/AspNetCore.Weixin/actions/workflows/test.yml/badge.svg)](https://github.com/myvas/AspNetCore.Weixin/actions/workflows/test.yml)
[![publish](https://github.com/myvas/AspNetCore.Weixin/actions/workflows/publish.yml/badge.svg)](https://github.com/myvas/AspNetCore.Weixin/actions/workflows/publish.yml)
[![NuGet](https://img.shields.io/nuget/v/Myvas.AspNetCore.Weixin.svg)](https://www.nuget.org/packages/Myvas.AspNetCore.Weixin)
[![NuGet](https://img.shields.io/nuget/v/Myvas.AspNetCore.Weixin.Jssdk.svg)](https://www.nuget.org/packages/Myvas.AspNetCore.Weixin.Jssdk)

WeixinApi services and WeixinSite middleware for Tencent Wechat/Weixin messages, events and apis. (微信公众平台/接口调用服务)

微信公众平台/接口调用服务：在微信公众平台上申请服务号或订阅号后，经配置部署可提供自定义菜单、即时信息交流、微信网页授权、模板消息通知等接口调用及搭建站点。

## NuGet Packages
1. Myvas.AspNetCore.Weixin

| Category | Method | Description | Options | Interfaces |
|-|-|-|-|-|
| 服务 | `services.AddWeixinAccessToken` | 微信公众号AccessToken | WeixinAccessTokenOptions | IWeixinAccessToken |
| 服务 | `services.AddWeixinWelcomePage` | 微信公众号消息处理 | WeixinWelcomePageOptions | |
| 中间件 | `app.UseWeixinWelcomePage` | 微信公众号消息处理 |  | IMessageHandler IWeixinMessageEncryptor |

2. Myvas.AspNetCore.Weixin.Jssdk

| Category | Method | Description | Options | Interfaces |
|-|-|-|-|-|
| 服务 | `services.AddWeixinJssdk` | 微信公众号Jssdk | WeixinJssdkOptions | IWeixinJsapiTicket |

## Demo
http://demo.auth.myvas.com (debian.9-x64) [![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/myvas/AspNetCore.Authentication.Demo?label=github)](https://github.com/myvas/AspNetCore.Authentication.Demo)


## 应用场景
1.接口服务WeixinApi，提供IWeixinAccessToken及其他接口服务
  * 拿到AccessToken后，想怎么用就怎么用
  * 使用经过二次设计构建的WeixinApi们，更方便
  * 搭建WebApi站点，供其他WebApp/MobileApp使用
```
services.AddWeixinApi(o => {
	o.AppId = "xxx";
	o.AppSecret = "xxx";
	//o.Backchannel = _testServer.CreateClient(); // default is 'new HttpClient()'
})
// default has already injected WeixinAccessTokenMemoryCacheProvider
//.AddCacheProvider<YourCacheProvider>() // or replaces by yours cache provider
;
```

优点：
* 通过单元测试：保证接口实现正确、发现功能迭代及代码重构过程产生的错误
* 良好的架构设计：提供新接口的可扩展性、改善旧接口的易用性
缺点：
* Too young too simple sometimes naive

2.中间件WeixinSite，用于搭建微信公众号服务站点
  * 接收微信公众号上行的消息和事件
  * 自动存储上述消息和事件
  * (客服)回复消息（须有上行消息，并在48小时内回复）
  * 发送模板消息（须预先定义并申请消息模板）
```
// 微信公众号服务站点
// ConfigureServices:
{
	services.AddWeixinSite(o => {
		o.AppId = "xxx"; 
		o.AppSecret = "xxx";
		o.SiteToken = "xxx";
	})
// 上下行消息加解密
	.AddEncoding(o => {
		o.StrictMode = true; // default is false (compatible with ClearText)
		o.AESEncodingKey = "xxx";
	})
// 上行消息及事件通知
	.AddEventSink<MyEventSink>()
// 自动存储上行消息及事件，允许自己设计及实现全部存储接口
	.AddStores<AppDbContext>(o => {
// 启用订阅者名单同步服务
		o.EnableSubscriberSync = false; // default is true
	})
// 接口服务：被动回复
	.AddResponseMessaging(o => {
		o.TrySmsOnFailed = true; // default is false
	})
// 接口服务：发送模板消息
	.AddTemplateMessaging(o => {
		o.MaxRetryTimes = 5; // default is 3
	})
	;
}

// Configure:
{
	app.UseWeixinSite();
}
```

3.(TBD)搭建AccessToken服务及管理服务器
  * 提供WebApi服务，供公司多个App取用
  * 对AccessToken调用流量进行监控和管理
  * 对公司App取用AccessToken的权限进行管理(建议使用Oidc或其他SSO进行授权管理)

## Demo
http://demo.auth.myvas.com (debian.9-x64) [![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/myvas/AspNetCore.Authentication.Demo?label=github)](https://github.com/myvas/AspNetCore.Authentication.Demo)

## Settings
https://mp.weixin.qq.com

1.开发/基本配置/公众号开发信息
- 获取**AppSecret**

2.开发/基本配置/服务器配置：**修改设置** | **启用**
- 在“服务器地址(**URL**)”中，填写地址: http://xxx.xxx/wx or https://xxx.xxx/wx
- 在“网站**Token**”中，填写一串较长的随机字符串作为WebsiteToken
- 在“消息加解密密钥**EncodingAESKey**”中，若空则初始化一个
- 在“消息加解密方式”中，***建议***选择“**安全模式**”

## AccessToken
* ConfigureServices
```
services.AddWeixinAccessToken(options => {	
	options.AppId = _configuration["Weixin:AppId"];
	options.AppSecret = _configuration["Weixin:AppSecret"];
});
```

* Usage
```
private readonly IAccessToken _accessToken;
...
var accessToken = _accessToken.GetTokenAsync();
```

## WeixinWelcomePage
* ConfigureServices
```
services.AddScoped<WeixinEventSink>();
var weixinEventSink = services.BuildServiceProvider().GetRequiredService<WeixinEventSink>();
services.AddWeixinWelcomePage(options =>
  {
      options.AppId = _configuration["Weixin:AppId"];
      options.AppSecret = _configuration["Weixin:AppSecret"];
      options.WebsiteToken = _configuration["Weixin:WebsiteToken"];
      
      options.EncodingAESKey = _configuration["Weixin:EncodingAESKey"]; //请注意检查该值正确无误！
      // （1）若填写错误，将导致您在启用“兼容模式”或“安全模式”时无法正确解密（及加密）；
      // （2）若您使用“微信公众平台测试号”部署，您应当注意到其不支持消息加解密，此时须用空字符串或不配置。
      
      options.Path = "/wx"; //默认值
      options.Debug = false; //默认值，不允许微信web开发者工具(wechatdevtools)等客户端访问。若修改为true则允许。
      
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

* Configure
```
app.UseWeixinWelcomePage();
```

## IWeixinAccessToken
* ConfigureServices
```
services.AddWeixinAccessToken(options => 
  {
      options.AppId = _configuration["Weixin:AppId"];
      options.AppSecret = _configuration["Weixin:AppSecret"];
  });
```

*  Controller
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

## AddWeixinJssdk, IWeixinJssdkTicket

* ConfigureServices
```
services.AddWeixinJssdk(options =>
{
      options.AppId = _configuration["Weixin:AppId"];
});
```

* Controller
```
private readonly IWeixinJsapiTicket _weixinJsapiTicket;
private readonly WeixinJssdkOptions _weixinJssdkOptions;

public IActionResult Index()
{
	var weixinJsConfig = new WeixinJsConfig()
	{
		debug = false,
		appId = _weixinJssdkOptions.AppId
	};

	var jsapiTicket = _weixinJsapiTicket.GetTicket();
	var refererUrl = Request.GetAbsoluteUri();
	var vm = new ShareJweixinViewModel
	{
		WeixinJsConfigJson = weixinJsConfig.ToJson(jsapiTicket, refererUrl),
		Title = "xxx",
		Url = Url.AbsoluteAction(...),
		Description = "yyy",
		ImgUrl = "https://...."
	};

	return View(vm);
}
```

* _Layout.cshtml
```
<script src="//res.wx.qq.com/open/js/jweixin-1.4.0.js" asp-append-version="true"></script>
```

* Index.cshtml
```
@model ShareJweixinViewModel

//...

<script type="text/javascript">
$(document).ready(function () {
	wx.config(@Html.Raw(Model.WeixinJsConfigJson);
	wx.ready(function(){
		wx.onMenuShareAppMessage({
			title: '@Html.Raw(Model.Title)', // 分享标题
			desc: '@Html.Raw(Model.Description)', // 分享描述
			link: '@Html.Raw(Model.Url)', // 分享链接
			imgUrl: '@Html.Raw(Model.ImgUrl)', // 分享图标
			type: '', // 分享类型,music、video或link，不填默认为link
			dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
			success: function () {
					// 用户确认分享后执行的回调函数
					toastSuccess("分享成功");
			},
			cancel: function () {
					// 用户取消分享后执行的回调函数
					toastWarning("已取消分享");
			}
		});
	});
</script>
```

## For Developers
* [Visual Studio 2022](https://visualstudio.microsoft.com)
* [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* [.NET 5.0](https://dotnet.microsoft.com/download/dotnet-core/5.0)
* [.NET 6.0](https://dotnet.microsoft.com/download/dotnet-core/6.0)
* [.NET 7.0](https://dotnet.microsoft.com/download/dotnet-core/7.0)
* [.NET 8.0](https://dotnet.microsoft.com/download/dotnet-core/8.0)
* [.NET 9.0](https://dotnet.microsoft.com/download/dotnet-core/9.0)
* [微信开发者工具](https://mp.weixin.qq.com/debug/wxadoc/dev/devtools/download.html)
* [微信公众平台](https://mp.weixin.qq.com)
