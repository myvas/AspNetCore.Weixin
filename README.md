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
// (1.1) AddWeixinCore(...) to inject WeixinMemoryCacheProvider, IWeixinAccessTokenApi, IWeixinJsapiTicketApi, IWeixinCardTicketApi.
// (1.2) AddWeixin(...) to inject WeixinMemoryCachProvider and all APIs (see the list below).
services.AddWeixin(o => {
	o.AppId = Configuration["Weixin:AppId"];
	o.AppSecret = Configuration["Weixin:AppSecret"];
	//o.Backchannel = _testServer.CreateClient(); // For testing using a fake TestServer
})

//(2.1) The default injection in AddWeixinCore and AddWeixin to provide a memory cache provider implemented IWeixinCacheProvider as default.
//.AddWeixinMemoryCacheProvider() 
// (2.2) To replace with a better distribution cache provider. (recommended)
.AddWeixinRedisCacheProvider(...)
// (2.3) Or, replace with your implementation of IWeixinCacheProvider.
//.AddWeixinCacheProvider<TWeixinCacheProvider>()
;
```

- 微信接口服务注入
  - `<IServiceCollection>.AddWeixin(Action<WeixinOptions>)`: 注入所有接口
  - `<IServiceCollection>.AddWeixinCore(Action<WeixinOptions>)`: 注入基础会话接口
- 基础会话接口：
  - `IWeixinAccessTokenApi`： [Usage](docs/Usages/IWeixinAccessTokenApi.md)
  - `IWeixinJsapiTicketApi`： [Usage](docs/Usages/IWeixinJsapiTicketApi.md)
  - `IWeixinCardTicketApi`: [Usage](docs/Usages/IWeixinCardTicketApi.md)
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
- Cache providers:
  - Memory cache provider: `AddWeixinMemoryCacheProvider` (Default injected in `AddWeixin(...)` and `AddWeixinCore(...)`)
  - Redis cache provider: `AddWeixinRedisCacheProvider(Action<RedisCacheOptions>)`
  - Customization of cache provider: `AddWeixinCacheProvider<TWeixinCacheProvider>` where `TWeixinCacheProvider` should implement `IWeixinCacheProvider` for `IWeixinExpirableValue` type.

## 微信公众号服务站点-中间件 `WeixinSiteMiddleware`

- Use the `WeixinSiteMiddleware`: 

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
  // (1) Add services for 
	.AddWeixinSite(o => {
		o.Path = Configuration.GetValue("Weixin:Path", "/wx"); // optional, default is "/wx"
		o.WebsiteToken = Configuration["Weixin:WebsiteToken"];
		o.Debug = Configuration.GetValue<bool>("Weixin:Debug", false); // optional, default is false (Do NOT allow `微信web开发者工具(wechatdevtools)` and other browsers to access)
	})
  
	// (2) 上下行消息加解密
	.AddMessageProtection(o => {
		o.EncodingAESKey = Configuration["Weixin:EncodingAESKey"];    
		o.StrictMode = Configuration.GetValue<bool>("Weixin:StrictMode", false); // default is false (compatible with ClearText)
		// (1) 若填写错误，将导致您在启用“兼容模式”或“安全模式”时无法正确解密（及加密）；
		// (2) 若您使用“微信公众平台测试号”部署，您应当注意到其不支持消息加解密，此时须用空字符串或不配置。
	})

  // (3.1) The default injection in AddWeixinSite to provide a debug output on received Weixin messages and events.
  //.AddWeixinDebugEventSink() 

  // (3.2) To replace with an implementation with persistance in database. 
	// 自动存储上行消息及事件
	.AddWeixinEfCore<TWeixinDbContext>(o => {
		// 启用订阅者名单同步服务
		o.EnableSyncForWeixinSubscribers = true; // default is false
		// 执行同步服务的时间间隔
		o.SyncIntervalInMinutesForWeixinSubscribers = 10; // min is 3 minutes
	})
	// 使用自定义数据类型
	//.AddWeixinEfCore<TWeixinDbContext, TWeixinSubscriber>(o => ...)
	//.AddWeixinEfCore<TWeixinDbContext, TWeixinSubscriber, TKey>(o => ...)

  // (3.3) Or, replace with your implementation of IWeixinEventSink.
  //.AddWeixinEventSink<TWeixinCacheProvider>()

	// (4) 接口服务：发送客服响应消息
	.AddWeixinPassiveResponseMessaging(o => {
		o.TrySmsOnFailed = true; // default is false
	})

	// (5) 接口服务：发送模板消息
	.AddWeixinTemplateMessaging(o => {
		o.MaxRetryTimes = 5; // default is 3
	});
	```
 
## Demo
http://demo.auth.myvas.com (debian.9-x64) [![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/myvas/AspNetCore.Authentication.Demo?label=github)](https://github.com/myvas/AspNetCore.Authentication.Demo)

## For Developers
### WeixinEfCoreEventSink
- [WeixinEfCoreEventSink](src/EfCore/EventSinks/WeixinEfCoreEventSink.cs)
- [WeixinTraceEventSink](src/Site/EventSinks/WeixinTraceEventSink.cs)
- [WeixinDebugEventSink](src/Site/EventSinks/WeixinDebugEventSink.cs)

### samples/WeixinSiteSample
1. Install the EF Core Tools (globally)
```
dotnet tool install --global dotnet-ef
```
2. Create Migrations (Run in dir: samples/WeixinSiteSample)
```
dotnet ef migrations add InitialCreate
```

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