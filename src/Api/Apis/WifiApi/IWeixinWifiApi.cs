using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinWifiApi
{
    Task<WeixinWifiErrorJson> BindAd(string adId, List<string> deviceNos, CancellationToken cancellationToken = default);
    Task<WeixinGetApplyShopResultJson> GetApplyShop(DateTime applyDate, CancellationToken cancellationToken = default);
    Task<WeixinGetAuditStatusResultJson> GetAuditStatus(string requestId, CancellationToken cancellationToken = default);
    Task<WeixinGetPublishedAdResultJson> GetPublishedAd(string deviceNo, CancellationToken cancellationToken = default);
    Task<WeixinGetQrcodeResultJson> GetQrcode(string accessToken, WeixinGetQrcodeParam param, CancellationToken cancellationToken = default);
    Task<WeixinGetWhiteListResultJson> GetWhiteList(CancellationToken cancellationToken = default);
    Task<WeixinWifiErrorJson> NoticeWeixinInfo(string mac, WeixinNoticeWeixinInfoAction action = WeixinNoticeWeixinInfoAction.push, string deviceMac = null, CancellationToken cancellationToken = default);
    Task<WeixinWifiErrorJson> PushApOnline(List<WeixinApOnlineData> apOnlineList, CancellationToken cancellationToken = default);
    Task<WeixinWifiErrorJson> PushStatData(DateTime statDate, List<DeviceStat> statList, CancellationToken cancellationToken = default);
    Task<WeixinSetAdResultJson> SetAdInfo(WeixinAd ad, CancellationToken cancellationToken = default);
    Task<WeixinSetApResultJson> SetApInfo(List<WeixinApInfo> apInfos, CancellationToken cancellationToken = default);
    Task<WeixinWifiErrorJson> SetVendorConfig(string callbackUrl, string callbackToken, CancellationToken cancellationToken = default);
}
