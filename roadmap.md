# Myvas.AspNetCore.Weixin 6.0.3

## 对微信公众号接口进行封装
在Api工程中实现微信公众号接口封装，依接口功能分组，命名为IWeixinXxxApi。

* 设计`WeixinJson`等基础类；依接口[文档](https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Get_access_token.html)设计子类，但将所有属性名称合规化（通过`JsonPropertyName`等）。
* 设计`WeixinException`等基础类：将错误类型分组，并设计子类。
* 其他工具类或扩展。

该工程无须配置，直接注入使用。注入方法为：`AddWeixinApi()`。

## 微信公众号接口调用凭证管理（WeixinAccessTokenManager）
在Core/Weixin工程中实现微信公众号接口调用凭证管理类（`WeixinAccessTokenManager`）。

* 将access_token分布式缓存统一管理，以便在多个项目（或程序集）中对同一个微信公众号进行管理时保持步调一致。
* 获取access_token时，先从redis中尝试获取，若未取得则从腾讯[调用](https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET)取得，并将其缓存到redis中（存续时间为expires_in）。

该工程在注入时配置（`WeixinOptions`），注入方法为：`AddWeixin(...)`。

## 微信订阅者管理（WeixinSubscriberManager<TSubscriber>）
在Core/Weixin工程中实现微信订阅者管理类（`WeixinSubscriberManager<TSubscriber>`）。

* 实现一个`WeixinSubscriberHostService`，用于周期性（例如每晚）获取微信订阅者信息并更新数据库表。还可以在每次程序启动时执行一次。
* 此外，结合在WeixinSite中间件中实现的订阅和退订事件，就可以较好地保持数据同步了。

该工程注入方法为：`AddSubscriberManager<TSubscriber, TDbContext>()` 。

## 微信站点（WeixinSite）
在Core/WeixinSite工程中实现。

* 实现微信数据包签名算法。
* 实现微信消息数据加密算法。
* 实现所有微信消息（包括事件）的解析算法。
* 设计微信消息及事件的处理函数注入接口（`IWeixinEventSink`），并实现一个基本功能示例（`WeixinEventSink`）：将接收到的所有消息（及事件）存储，在接收到订阅及退订事件时更新微信订阅者数据库表。

该工程在注入时配置（`WeixinSiteOptions`），指定微信公众号消息加密参数等。

* 注入方法为：`AddSite<TDbContext, TEventSink>()`。
* 使用方法为：`UseSite(...)`，可以在参数中指定服务地址（默认为"/wx"）。

## [微信JS-SDK](https://developers.weixin.qq.com/doc/offiaccount/OA_Web_Apps/JS-SDK.html)接口调用凭证管理（WeixinJsapiTicketManager）
在Jssdk/WeixinJsapiTicketManager工程中实现微信JS接口凭证管理（`WeixinJsapiTicketManager`）。

* 将jsapi ticket分布式缓存统一管理，以便在多个项目（或程序集）中对同一个微信公众号进行管理时保持步调一致。
* 获取jsapi ticket时，先从redis中尝试获取，若未取得则从腾讯[调用](https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=ACCESS_TOKEN&type=jsapi)取得，并将其缓存到redis中（存续时间为expires_in）。

该工程依赖`WeixinAccessTokenManager`，因为调用接口获取jsapi ticket时须提供access_token。

该工程注入方法为：`AddWeixinJsapiTicket()`。

## [微信卡券](https://developers.weixin.qq.com/doc/offiaccount/OA_Web_Apps/JS-SDK.html#54)接口调用凭证管理（WeixinJssdkCardTicketManager）
在Jssdk/WeixinCardTicketManager工程中实现微信卡券接口调用凭证管理（`WeixinJssdkCardTicketManager`）。

* 将wx_card ticket分布式缓存统一管理，以便在多个项目（或程序集）中对同一个微信公众号进行管理时保持步调一致。
* 获取wx_card ticket时，先从redis中尝试获取，若未取得则从腾讯[调用](https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=ACCESS_TOKEN&type=wx_card)取得，并将其缓存到redis中（存续时间为expires_in）。

该工程依赖`WeixinAccessTokenManager`，因为调用接口获取wx_card ticket时须提供access_token。

该工程注入方法为：
`AddWeixinCardTicket()`。