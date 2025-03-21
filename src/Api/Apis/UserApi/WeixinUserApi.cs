using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 用户接口
/// <remarks>接口详见：http://mp.weixin.qq.com/wiki/index.php?title=%E8%8E%B7%E5%8F%96%E7%94%A8%E6%88%B7%E5%9F%BA%E6%9C%AC%E4%BF%A1%E6%81%AF</remarks>
/// </summary>
public class WeixinUserApi : WeixinSecureApiClient, IWeixinUserApi
{
    public WeixinUserApi(IOptions<WeixinOptions> optionsAccessor, IWeixinAccessTokenApi tokenProvider) : base(optionsAccessor, tokenProvider)
    {
    }

    /// <summary>
    /// 获取微信用户资料
    /// </summary>
    /// <param name="accessToken">微信访问凭证</param>
    /// <param name="openId">微信用户标识。在同一公众号中，一个微信号（用户）每次订阅后得到的OpenId都是唯一的。</param>
    /// <param name="lang"><see cref="WeixinLanguage"/></param>
    /// <returns>微信用户资料</returns>
    /// <remarks>
    /// 错误时微信会返回错误码等信息，JSON数据包示例如下（该示例为AppID无效错误）:
    /// <code>
    /// {"errcode":40013,"errmsg":"invalid appid"}
    /// </code>
    /// </remarks>
    public async Task<WeixinUserInfoJson> Info(string openId, string lang = "zh_CN", CancellationToken cancellationToken = default)
    {
        var accessToken = await GetTokenAsync(cancellationToken);

        var pathAndQuery = "/cgi-bin/user/info?access_token={0}&openid={1}&lang={2}";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        url = string.Format(url, accessToken, openId, lang.ToString());

        return await GetFromJsonAsync<WeixinUserInfoJson>(url);
    }

    public Task<WeixinUserInfoJson> Info(string openId, WeixinLanguage lang, CancellationToken cancellationToken = default)
        => Info(openId, lang.Code, cancellationToken);

    /// <summary>
    /// 获取一个批次的订阅者的OpenId
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="nextOpenId"></param>
    /// <returns></returns>
    public async Task<WeixinUserGetJson> Get(string nextOpenId, CancellationToken cancellationToken = default)
    {
        var accessToken = await GetTokenAsync(cancellationToken);

        var pathAndQuery = "/cgi-bin/user/get?access_token={0}";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        url = string.Format(url, accessToken);
        if (!string.IsNullOrEmpty(nextOpenId))
        {
            url += "&next_openid=" + nextOpenId;
        }
        return await GetFromJsonAsync<WeixinUserGetJson>(url);
    }

    /// <summary>
    /// 获取所有订阅者的OpenId
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    public async Task<List<string>> GetAllOpenIds(CancellationToken cancellationToken = default)
    {
        List<string> openids = new List<string>();

        string nextOpenId = "";
        do
        {
            WeixinUserGetJson followerResult = await Get(nextOpenId);
            int count = followerResult.count;
            if (count > 0)
            {
                openids.AddRange(followerResult.data.openid);
            }
            nextOpenId = followerResult.next_openid;
        } while (!string.IsNullOrEmpty(nextOpenId));

        return openids;
    }

    /// <summary>
    /// 获取所有订阅者的资料
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    public async Task<List<WeixinUserInfoJson>> GetAllUserInfo(CancellationToken cancellationToken = default)
    {
        List<WeixinUserInfoJson> subscribers = new List<WeixinUserInfoJson>();

        List<string> openids = await GetAllOpenIds();
        foreach (string openid in openids)
        {
            try
            {
                WeixinUserInfoJson userInfo = await Info(openid);
                if (userInfo != null)
                {
                    subscribers.Add(userInfo);
                }
            }
            catch {
            }
        }

        return subscribers;
    }
}
