# Myvas.AspNetCore.Weixin

[![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/myvas/AspNetCore.Weixin?label=github)](https://github.com/myvas/AspNetCore.Weixin)
[![test](https://github.com/myvas/AspNetCore.Weixin/actions/workflows/test.yml/badge.svg)](https://github.com/myvas/AspNetCore.Weixin/actions/workflows/test.yml)
[![publish](https://github.com/myvas/AspNetCore.Weixin/actions/workflows/publish.yml/badge.svg)](https://github.com/myvas/AspNetCore.Weixin/actions/workflows/publish.yml)
[![NuGet](https://img.shields.io/nuget/v/Myvas.AspNetCore.Weixin.svg)](https://www.nuget.org/packages/Myvas.AspNetCore.Weixin)
[![NuGet](https://img.shields.io/nuget/vpre/Myvas.AspNetCore.Weixin.svg)](https://www.nuget.org/packages/Myvas.AspNetCore.Weixin)

This solution is working around the Tencent WeChat (also known as Weixin) platform APIs, designed to streamline integration and enhance developer productivity.

微信公众平台/接口调用服务：在微信公众平台上申请服务号或订阅号后，经配置部署可提供自定义菜单、即时信息交流、微信网页授权、模板消息通知等接口调用及搭建站点。

## 获取配置参数 Options

微信公众平台：https://mp.weixin.qq.com

1. 开发/基本配置/公众号开发信息

   - 获取**AppId**，作为参数`<WeixinOptions>.AppId`
   - 获取**AppSecret**，作为参数`<WeixinOptions>.AppSecret`

2. 开发/基本配置/服务器配置：**修改设置** | **启用**

   - 在“服务器地址(**URL**)”中，填写地址: http://xxx.xxx/wx or https://xxx.xxx/wx，将二级虚拟路径(`"/wx"`)作为参数`<WeixinSiteOptions>.Path`
   - 在“网站**Token**”中，填写一串较长的随机字符串，作为参数`<WeixinSiteOptions>.WebsiteToken`
   - 在“消息加解密密钥**EncodingAESKey**”中，若空则初始化一个，作为参数`<WeixinMessageProtectionOptions>.EncodingAESKey`
   - 在“消息加解密方式”中，***建议***选择“**安全模式**”。其他选项有"**明文模式**"和"**兼容模式**"。当且仅当您选择"**安全模式**"时，将参数`<WeixinMessageProtectionOptions>.StrictMode`设置为`true`。

## 微信接口服务容器 `WeixinBuilder`

```csharp
// Optional: AddWeixinCore(...) for IWeixinAccessTokenApi and WeixinMemoryCacheProvider only.
services.AddWeixin(o => {
	o.AppId = Configuration["Weixin:AppId"];
	o.AppSecret = Configuration["Weixin:AppSecret"];
	//o.Backchannel = _testServer.CreateClient(); // For testing on your FakeServer
})
// WeixinMemoryCacheProvider has already injected as the default.
// WeixinRedisCacheProvider is also a better choice for distribution cache.
// or replace by your customized implementation of IWeixinCacheProvider.
//.AddWeixinCacheProvider<YourCacheProvider>()
;
```

- 微信接口服务注入
  - `<IServiceCollection>.AddWeixin(Action<WeixinOptions>)`: 注入所有接口
  - `<IServiceCollection>.AddWeixinCore(Action<WeixinOptions>)`: 注入基础会话接口
- 基础会话接口：
  - `IWeixinAccessTokenApi`： [Usage](docs/Usages/IWeixinAccessTokenApi.md)
  - `IWeixinJsapiTicketApi`： [Usage](docs/Usages/IWeixinJsapiTicketApi.md)
  - `IWeixinCardApiTicketApi`: 
- 数据管理接口：
  - `IWeixinUserApi`: 
  - `IWeixinUserProfileApi`: 
  - `IWeixinUserGroupApi`: 
  - `IWeixinGroupApi`: 
- 其他业务接口：
  - `IWeixinCommonApi`: 
  - `IWeixinMenuApi`: 
  - `IWeixinMediaApi`: 
  - `IWeixinCustomerSupportApi`: 
  - `IWeixinGroupMessageApi`: 
  - `IWeixinQrcodeApi`: 
  - `IWeixinWifiApi`: 
- Generic Cache Providers for `TWeixinCacheJson` : `IWeixinCacheJson`
  - `WeixinMemoryCacheProvider<TWeixinCacheJson>` (Default injected in `AddWeixin(...)` and `AddWeixinCore(...)`)
    - `<WeixinBuilder>.AddWeixinMemoryCacheProvider<TWeixinCacheJson>()`
  - `WeixinRedisCacheProvider<TWeixinCacheJson>`
    - `<WeixinBuilder>.AddWeixinRedisCacheProvider<TWeixinCacheJson>(Action<RedisCacheOptions>)`
  - Customized Interface type: `IWeixinCacheProvider<TWeixinCacheJson>`
    - XxxCacheProviderOptions: `TOptions`
    - XxxCacheProviderPostConfigureOptions: `IPostConfigureOptions<TOptions>`
    - WeixinBuilderXxxCacheProviderExtensions: `<WeixinBuilder>.AddWeixinCacheProvider<TWeixinCacheJson, TWeixinCacheProvider>(Action<TWeixinCacheProviderOptions> setupAction = null)`

## 微信公众号服务站点-中间件 `WeixinSiteMiddleware`

- Use Middleware: `<IApplicationBuilder>.UseMiddleware<WeixinSiteMiddleware>`

	```csharp
	app.UseWeixinSite();
	```

- 用于搭建微信公众号服务站点
  - 接收微信公众号上行的消息和事件: [Usage](docs/Usages/WeixinEventSink.md)
  - 发送(客服)响应类消息（须有上行消息，并在48小时内回复）
  - 发送模板消息（须预先定义并申请消息模板），模板存储及管理

## 微信公众号服务站点-接口服务容器 `WeixinSiteBuilder`

- Dependency Injection: `IServiceCollection`

	```csharp
	// <WeixinBuilder>
	.AddWeixinSite(o => {
		o.Path = Configuration["Weixin:Path"];
		o.WebsiteToken = Configuration["Weixin:WebsiteToken"];
		o.Debug = false; //默认为false，即：不允许微信web开发者工具(wechatdevtools)等客户端访问。若修改为true则允许。
	})

	// 上下行消息加解密
	.AddWeixinMessageProtection(o => {
		o.StrictMode = true; // default is false (compatible with ClearText)
		o.EncodingAESKey = Configuration["Weixin:EncodingAESKey"];//请注意检查该值正确无误！
		// （1）若填写错误，将导致您在启用“兼容模式”或“安全模式”时无法正确解密（及加密）；
		// （2）若您使用“微信公众平台测试号”部署，您应当注意到其不支持消息加解密，此时须用空字符串或不配置。
	});

	// 上行消息及事件通知，已默认注入`WeixinEventSink`
	//.AddWeixinEventSink<TWeixinEventSink>()

	// 接口服务：发送模板消息
	.AddWeixinTemplateMessaging(o => {
		o.MaxRetryTimes = 5; // default is 3
	})

	// 接口服务：发送客服响应消息
	.AddWeixinPassiveResponseMessaging(o => {
		o.TrySmsOnFailed = true; // default is false
	})

	// 自动存储上行消息及事件
	.AddWeixinEntityFrameworkCore(o => {
		// 启用订阅者名单同步服务
		o.EnableSubscriberSync = false; // default is true
	})
	// 自定义数据类型
	//.AddWeixinEntityFrameworkCore<TWeixinSubscriber, TWeixinDbContext>(Action<TWeixinEntityFrameworkCoreOptions>);
	```
 
## Demo
http://demo.auth.myvas.com (debian.9-x64) [![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/myvas/AspNetCore.Authentication.Demo?label=github)](https://github.com/myvas/AspNetCore.Authentication.Demo)

## For Developers
* [Visual Studio 2022](https://visualstudio.microsoft.com)  
  - [Tools/ResX Manager](https://marketplace.visualstudio.com/items?itemName=TomEnglert.ResXManager)
* [Visual Studio Code](https://code.visualstudio.com)
  - C#, IntelliCode, .NET Install Tool (Microsoft)
  - XML Tools (Josh Johnson)
  - .NET Core User Secrets (Adrian Wilczyński)
  - ResX Editor (Dominic Vonk)
  - Markdown All in One (Yu Zhang)
* Testing on:
  - [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
  - [.NET 5.0](https://dotnet.microsoft.com/download/dotnet-core/5.0)
  - [.NET 6.0](https://dotnet.microsoft.com/download/dotnet-core/6.0)
  - [.NET 7.0](https://dotnet.microsoft.com/download/dotnet-core/7.0)
  - [.NET 8.0](https://dotnet.microsoft.com/download/dotnet-core/8.0)
  - [.NET 9.0](https://dotnet.microsoft.com/download/dotnet-core/9.0)
* [微信开发者工具](https://mp.weixin.qq.com/debug/wxadoc/dev/devtools/download.html)
* [微信公众平台](https://mp.weixin.qq.com)