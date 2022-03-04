# Myvas.AspNetCore.Weixin [![NuGet](https://img.shields.io/nuget/v/Myvas.AspNetCore.Weixin.svg)](https://www.nuget.org/packages/Myvas.AspNetCore.Weixin) [![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/myvas/AspNetCore.Weixin?label=github)](https://github.com/myvas/AspNetCore.Weixin)
An ASP.NET Core middleware for Tencent Wechat/Weixin message handling and apis. (微信公众平台/接口调用服务)

微信公众平台/接口调用服务：在微信公众平台上申请服务号或订阅号后，经配置部署可提供自定义菜单、即时信息交流、微信网页授权、模板消息通知等接口调用服务。

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

## How to Configure?
* ConfigureServices
```
services
	//IWeixinApi: utilities to communicate with the tencent server.
	.AddWeixin(o =>
	{
		o.AppId = Configuration["Weixin:AppId"];
		o.AppSecret = Configuration["Weixin:AppSecret"];
	})
	//IWeixinAccessToken: get access_token cached in redis, or fetch it from the remote.
	.AddAccessToken(o =>
	{
		o.Configuration = Configuration.GetConnectionString("RedisConnection");
		o.InstanceName = Configuration["Weixin:AppId"];
	})
	//Weixin site handlers
	.AddWeixinSite<DefaultWeixinEventSink, ApplicationUser, ApplicationDbContext>(o =>
	{
		o.WebsiteToken = builder.Configuration["Weixin:WebsiteToken"];

		//是否允许微信web开发者工具(wechatdevtools)等客户端访问？默认值为false，true为允许。
		o.Debug = bool.Parse(builder.Configuration["Weixin:Debug"] ?? "false");
	});
```

* Configure
```
app.UseWeixinSite(); //Path默认为"/wx"
//app.UseWeixinSite("/wx"); //与上一行等效
```

## How to Use: IWeixinAccessToken?
*  XxxController
```
private readonly IWeixinAccessToken _weixinAccessToken;

public XxxController(IWeixinAccessToken weixinAccessToken)
{
    _weixinAccessToken = weixinAccessToken;
}

public IActionResult MethodA()
{
   var token = _weixinAccessToken.GetToken();
}
```

## How to Use: IWeixinJssdkTicket
* XxxController
```
private readonly IWeixinJsapiTicket _ticket;
private readonly WeixinJssdkOptions _options;

public XxxController(IWeixinJsapiTicket ticket, IOptions<WeixinJssdkOptions> optionsAccessor)
{
	_ticket = ticket ?? throw new ArgumentNullException(nameof(ticket));
	_options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
}

public IActionResult Index()
{
	var weixinJsConfig = new WeixinJsConfig()
	{
		debug = false,
		appId = _options.AppId
	};

	var jsapiTicket = _ticket.GetTicket();
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

## Dev
* [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [微信开发者工具](https://mp.weixin.qq.com/debug/wxadoc/dev/devtools/download.html)
