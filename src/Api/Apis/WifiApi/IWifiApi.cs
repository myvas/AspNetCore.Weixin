using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWifiApi
{
    Task<WifiErrorJson> BindAd(string adId, List<string> deviceNos, CancellationToken cancellationToken = default);
    Task<GetApplyShopResultJson> GetApplyShop(DateTime applyDate, CancellationToken cancellationToken = default);
    Task<GetAuditStatusResultJson> GetAuditStatus(string requestId, CancellationToken cancellationToken = default);
    Task<GetPublishedAdResultJson> GetPublishedAd(string deviceNo, CancellationToken cancellationToken = default);
    Task<GetQrcodeResultJson> GetQrcode(string accessToken, GetQrcodeParam param, CancellationToken cancellationToken = default);
    Task<GetWhiteListResultJson> GetWhiteList(CancellationToken cancellationToken = default);
    Task<WifiErrorJson> NoticeWeixinInfo(string mac, NoticeWeixinInfoAction action = NoticeWeixinInfoAction.push, string deviceMac = null, CancellationToken cancellationToken = default);
    Task<WifiErrorJson> PushApOnline(List<ApOnlineData> apOnlineList, CancellationToken cancellationToken = default);
    Task<WifiErrorJson> PushStatData(DateTime statDate, List<DeviceStat> statList, CancellationToken cancellationToken = default);
    Task<SetAdResultJson> SetAdInfo(Ad ad, CancellationToken cancellationToken = default);
    Task<SetApResultJson> SetApInfo(List<ApInfo> apInfos, CancellationToken cancellationToken = default);
    Task<WifiErrorJson> SetVendorConfig(string callbackUrl, string callbackToken, CancellationToken cancellationToken = default);
}
