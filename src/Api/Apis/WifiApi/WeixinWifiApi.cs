﻿using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 微信扫一扫WiFi接口(V0.8)
/// <para>微信扫一扫Wi-Fi是解决传统商务Wi-Fi连接授权认证的一个方案，代替传统web认证需要用户输入用户名、密码等信息的过程，并在微信界面给予有安全性认证的Wi-Fi服务提供商一个信息展示广告位的入口，以充实其商业化价值。</para>
/// <para>微信扫一扫只是解决微信用户连接Wi-Fi通过授权认证的一个手段，Wi-Fi的安全性需要Wi-Fi服务提供商负责。</para>
/// </summary>
/// <remarks>
/// <list type="">接口实现说明
/// <item>接口1：已在公共接口(<see cref="WeixinAccessTokenDirectApi"/>)中实现</item>
/// <item>接口4：通过邮件方式发送</item>
/// <item>接口8：在网站应用程序中实现</item>
/// </list>
/// </remarks>
public class WeixinWifiApi : SecureWeixinApiClient, IWeixinWifiApi
{
    public WeixinWifiApi(IOptions<WeixinOptions> optionsAccessor, IWeixinAccessTokenApi tokenProvider) : base(optionsAccessor, tokenProvider)
    {
    }

    /// <summary>
    /// 接口2.获取微信WiFi二维码图片
    /// </summary>
    /// <returns>二维码图片url</returns>
    public async Task<WeixinGetQrcodeResultJson> GetQrcode(string accessToken, WeixinGetQrcodeParam param, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/wifi/getQRCode.xhtml?access_token=ACCESS_TOKEN&format=json";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        Dictionary<string, string> formData = new Dictionary<string, string>();
        formData.Add("ssid", param.ssid);
        formData.Add("lgnId", param.lgnId);
        formData.Add("templateId", param.templateId.ToString());
        formData.Add("bgUrl", param.bgUrl);
        formData.Add("qrType", param.qrType.ToString());
        formData.Add("storeId", param.storeId);
        formData.Add("expireTime", param.expireTime.ToString());

        WeixinGetQrcodeResultJson result = await SecurePostFormAsJsonAsync<WeixinGetQrcodeResultJson>(url, formData);
        return result;
    }

    /// <summary>
    /// 接口3.获取AP白名单信息
    /// <para>AP需要针对微信的特殊授权请求作放行，必须获取到放行的微信接入ip。</para>
    /// </summary>
    /// <returns>白名单</returns>
    public async Task<WeixinGetWhiteListResultJson> GetWhiteList(CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/wifi/getWhiteList.xhtml?access_token=ACCESS_TOKEN&format=json";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        WeixinGetWhiteListResultJson result = await SecureGetFromJsonAsync<WeixinGetWhiteListResultJson>(url);
        return result;
    }

    /// <summary>
    /// 接口5.设置AP信息
    /// </summary>
    /// <remarks>Wi-Fi服务提供商安装门店AP完成后，必须把AP信息设置到微信系统中来。</remarks>
    /// <param name="accessToken"></param>
    /// <param name="apInfos"></param>
    /// <returns>true:设置成功。</returns>
    public async Task<WeixinSetApResultJson> SetApInfo(List<WeixinApInfo> apInfos, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/wifi/setApInfo.xhtml?access_token=ACCESS_TOKEN&format=json";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        string json = JsonSerializer.Serialize(apInfos);
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("apList", json);

        WeixinSetApResultJson result = await SecurePostFormAsJsonAsync<WeixinSetApResultJson>(url, param);
        return result;
    }

    /// <summary>
    /// 接口6.设定广告内容
    /// <para>Wi-Fi服务提供商安装门店AP完成后，必须把AP信息设置到微信系统中来。</para>
    /// <para>Wi-Fi服务提供商为门店AP设置广告信息。设置成功后，并不是马上生效。需要微信运营审核通过再生效。一个门店的若干AP设置同一个广告的情况, 通过deviceNos传多个进来。</para>
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="ad">广告内容。
    /// <para>注意：有两个模板，这两个模板的数据格式略有差异。</para>
    /// <para>建议：不要直接设置adTemplet和adDetail属性值。</para>
    /// <para>建议：调用Ad.SetAdDetail(...)来设置广告内容的细节！</para></param>
    /// <returns></returns>
    public async Task<WeixinSetAdResultJson> SetAdInfo(WeixinAd ad, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/wifi/setAdInfo.xhtml?access_token=ACCESS_TOKEN&format=json";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        Dictionary<string, string> param = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(ad.advertiseId)) param.Add("advertiseId", ad.advertiseId);
        if (!string.IsNullOrEmpty(ad.logo)) param.Add("logo", ad.logo);
        if (!string.IsNullOrEmpty(ad.backgroundImg)) param.Add("backgroundImg", ad.backgroundImg);
        if (!string.IsNullOrEmpty(ad.brandName)) param.Add("brandName", ad.brandName);
        if (!string.IsNullOrEmpty(ad.mpAccount)) param.Add("mpAccount", ad.mpAccount);
        param.Add("deviceNos", JsonSerializer.Serialize(ad.deviceNos));
        param.Add("adTemplet", ad.adTemplet.ToString());
        param.Add("adDetail", ad.adDetail);

        WeixinSetAdResultJson result = await SecurePostFormAsJsonAsync<WeixinSetAdResultJson>(url, param);
        return result;
    }

    /// <summary>
    /// 接口7.通知微信触发认证
    /// </summary>
    /// <remarks>当用户未经扫一扫而手动连接Wi-Fi，或者用户之前设备记忆自动连接Wi-Fi后，发起网络请求不通过，Wi-Fi服务提供商通知微信触发验证通知时调用。
    /// <para>当用户成功连接登录授权AP网络时，Wi-Fi服务提供商通知微信用户登录结果。</para></remarks>
    /// <param name="accessToken">调用接口凭证</param>
    /// <param name="action">通知触发的行为，目前有push，loginSuccess，loginFail。详见<see cref="WeixinNoticeWeixinInfoAction"/></param>
    /// <param name="mac">已经联网触发网络请求的设备手机mac地址</param>
    /// <param name="deviceMac">已经联网触发网络请求的AP设备mac地址
    /// <para>(action为loginSuccess，loginFail时必须输入该该参数)</para></param>
    /// <returns></returns>
    public async Task<WeixinWifiErrorJson> NoticeWeixinInfo(string mac, WeixinNoticeWeixinInfoAction action = WeixinNoticeWeixinInfoAction.push, string deviceMac = null, CancellationToken cancellationToken = default)
    {
        var accessToken = await GetTokenAsync(cancellationToken);

        var pathAndQuery = "/wifi/noticeWeixinInfo.xhtml?access_token=ACCESS_TOKEN&action=ACTION&mac=CLIENTMAC&deviceMac=DEVICEMAC&format=json";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        url = url.Replace("ACCESS_TOKEN", accessToken);
        url = url.Replace("ACTION", action.ToString());
        url = url.Replace("CLIENTMAC", mac);
        url = url.Replace("DEVICEMAC", deviceMac);

        WeixinWifiErrorJson result = await GetFromJsonAsync<WeixinWifiErrorJson>(url);
        return result;
    }

    /// <summary>
    /// 接口9.拉取审核结果
    /// <para>当Wi-Fi服务提供商调用(1)设置广告，或者(2)生成手机二维码接口请求，微信运营人员会对内容进行审核。</para>
    /// <para>Wi-Fi服务提供商调用该接口获取审核状态。</para>
    /// </summary>
    /// <param name="accessToken">调用接口凭证</param>
    /// <param name="requestId">审核结果对应的请求id，该请求id设置AD接口会返回</param>
    /// <param name="type">审核结果类型，预留</param>
    /// <returns>审核结果</returns>
    public async Task<WeixinGetAuditStatusResultJson> GetAuditStatus(string requestId, CancellationToken cancellationToken = default)
    {
        var accessToken = await GetTokenAsync(cancellationToken);

        var pathAndQuery = "/wifi/getAuditStatus.xhtml?access_token=ACCESS_TOKEN&requestId=REQUESTID&format=json";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        url = url.Replace("ACCESS_TOKEN", accessToken);
        url = url.Replace("REQUESTID", requestId);
        //api = api.Replace("type", auditResultType);

        WeixinGetAuditStatusResultJson result = await GetFromJsonAsync<WeixinGetAuditStatusResultJson>(url);
        return result;
    }

    /// <summary>
    /// 接口10.获取商家WiFi申请信息
    /// <para>微信侧的商户选择设备商，并提交申请开通扫一扫WIFI功能后，设备商可以通过该接口拉取申请的商户信息，之后线下为商户安装AP设备，并通过设置AP接口同步过来。</para>
    /// </summary>
    /// <param name="accessToken">调用接口凭证</param>
    /// <param name="applyDate">商户申请的日期，格式20140716</param>
    /// <returns>商家WiFi申请信息</returns>
    public async Task<WeixinGetApplyShopResultJson> GetApplyShop(DateTime applyDate, CancellationToken cancellationToken = default)
    {
        var accessToken = await GetTokenAsync(cancellationToken);

        var pathAndQuery = "/wifi/getApplyShop.xhtml?access_token=ACCESS_TOKEN&applyDate=APPLYDATE&format=json";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        url = url.Replace("ACCESS_TOKEN", accessToken);
        url = url.Replace("APPLYDATE", applyDate.ToString("yyyyMMdd"));

        WeixinGetApplyShopResultJson result = await GetFromJsonAsync<WeixinGetApplyShopResultJson>(url);
        return result;
    }

    /// <summary>
    /// 接口11.推送AP统计数据到微信
    /// <para>为保证用户体验，微信需要对服务商的连接成功率进行监控，需要AP相关的统计数据支持。WI-FI服务商每天推送前一天的AP统计数据给到微信。</para>
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="statDate">Ap的统计数据日期，格式是20140903</param>
    /// <param name="statList">统计数据</param>
    /// <returns>正常情况下，微信会返回下述JSON数据包给开发者：
    /// <para><code>{“errorCode”:0, ”errorMessage”:”” }</code></para>
    /// 错误时微信会返回错误码和错误的设备编码等信息， JSON数据包示例如下:
    /// <para><code>{"errorCode":30013,"errorMessage":"参数非法"}</code></para>
    /// </returns>
    public async Task<WeixinWifiErrorJson> PushStatData(DateTime statDate, List<DeviceStat> statList, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/wifi/pushStatData.xhtml?access_token=ACCESS_TOKEN&format=json";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("statDate", statDate.ToString("yyyyMMdd"));
        string statListJson = JsonSerializer.Serialize(statList);
        param.Add("statList", statListJson);

        WeixinWifiErrorJson result = await SecurePostFormAsJsonAsync<WeixinWifiErrorJson>(url, param);
        return result;
    }

    /// <summary>
    /// 接口12. 获取线上的AD
    /// <para>Wi-Fi运营商可以根据需要通过该接口读取商家主页线上的url。</para>
    /// </summary>
    /// <param name="accessToken">调用接口凭证</param>
    /// <param name="deviceNo">需要查询的ap设备号</param>
    /// <returns>正常情况下，微信会返回下述JSON数据包给开发者：
    /// <para><code>{“errorCode”:0,”errorMessage”:””, “adUrl” : “http://imgurl” }</code></para>
    /// 错误时微信会返回错误码等信息，JSON数据包示例如下（该示例为AppID无效错误）:
    /// <para><code>{"errorCode":40013,"errorMessage":"invalid appid"}</code></para></returns>
    public async Task<WeixinGetPublishedAdResultJson> GetPublishedAd(string deviceNo, CancellationToken cancellationToken = default)
    {
        var accessToken = await GetTokenAsync(cancellationToken);

        var pathAndQuery = "/wifi/getPublishedAd.xhtml?access_token=ACCESS_TOKEN&deviceNo=DEVICENOformat=json";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        url = url.Replace("ACCESS_TOKEN", accessToken);
        url = url.Replace("DEVICENO", deviceNo);

        WeixinGetPublishedAdResultJson result = await GetFromJsonAsync<WeixinGetPublishedAdResultJson>(url);
        return result;
    }

    /// <summary>
    /// 接口13.设置WiFi服务商配置信息
    /// <para>当Wi-Fi服务提供商完成回调授权接口的实现后，需要将该接口的url和接口凭证设置到微信系统中来。</para>
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="callbackUrl">回调授权的URL
    /// <para>例如：“http://xxxx”</para></param>
    /// <param name="callbackToken">调用callbackUrl的凭证，即接口8(回调授权请求)中生成signature的token</param>
    /// <returns>正常情况下，微信会返回下述JSON数据包给开发者：
    /// <para><code>{"errorCode":0, "errorMessage":"" }</code></para>
    /// 错误时微信会返回错误码等信息，JSON数据包示例如下（该示例为AppID无效错误）:
    /// <para><code>{"errorCode":40013,"errorMessage":"invalid appid"}</code></para>
    /// </returns>
    public async Task<WeixinWifiErrorJson> SetVendorConfig(string callbackUrl, string callbackToken, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/wifi/setVendorConfig.xhtml?access_token=ACCESS_TOKEN&format=json";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("callbackUrl", callbackUrl);
        param.Add("token", callbackToken);

        WeixinWifiErrorJson result = await SecurePostFormAsJsonAsync<WeixinWifiErrorJson>(url, param);
        return result;
    }


    /// <summary>
    /// 接口14. 推送AP心跳数据到微信
    /// <para>为更好的监控AP实时在线运营情况，以便给用户更好稳定的上网体验。微信需要AP心跳数据的支持。</para>
    /// <para>请WI-FI服务商每5分钟推送AP当前在线状态以及连接人数给到微信。</para>
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns>正常情况下，微信会返回下述JSON数据包给开发者：
    /// <para><code>{"errorCode":0, "errorMessage":"" }</code></para>
    /// 错误时微信会返回错误码等信息，JSON数据包示例如下（该示例为AppID无效错误）:
    /// <para><code>{"errorCode":40013,"errorMessage":"invalid appid"}</code></para>
    /// </returns>
    public async Task<WeixinWifiErrorJson> PushApOnline(List<WeixinApOnlineData> apOnlineList, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/wifi/pushApOnline.xhtml?access_token=ACCESS_TOKEN&format=json";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("apOnlineList", JsonSerializer.Serialize(apOnlineList));

        WeixinWifiErrorJson result = await SecurePostFormAsJsonAsync<WeixinWifiErrorJson>(url, param);
        return result;
    }

    /// <summary>
    /// 接口15. 绑定已生成的AD到AP
    /// <para>为方便开发者将已经设置过的广告应用到其他AP上，该接口只需提供之前设置过的广告ID以及希望应用于的AP的设备编号列表。</para>
    /// <para>接口6设置AD信息会返回设置好的广告ID：requestId，如果想将该广告应用到其他AP上，则可以传该ID。</para>
    /// <para>如果该广告审核不通过，则返回失败。 如果该广告审核不通过，则返回失败。</para>
    /// </summary>
    /// <param name="accessToken">调用接口凭证</param>
    /// <param name="adId">广告ID，即接口6设置AD信息成功后返回的requestId</param>
    /// <param name="deviceNos">AP设备编号列表，json数组串。
    /// <para>例如：</para>
    /// <para><code>["deviceno1","deviceno2"]</code></para></param>
    /// <returns>正常情况下，微信会返回下述JSON数据包给开发者：
    /// <para><code>{"errorCode":0, "errorMessage":"" }</code></para>
    /// 错误时微信会返回错误码等信息，JSON数据包示例如下（该示例为AppID无效错误）:
    /// <para><code>{"errorCode":40013,"errorMessage":"invalid appid"}</code></para>
    /// </returns>
    public async Task<WeixinWifiErrorJson> BindAd(string adId, List<string> deviceNos, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/wifi/bindAd.xhtml?access_token=ACCESS_TOKEN&format=json";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("adId", adId);
        param.Add("deviceNos", JsonSerializer.Serialize(deviceNos));

        WeixinWifiErrorJson result = await SecurePostFormAsJsonAsync<WeixinWifiErrorJson>(url, param);
        return result;
    }
}
