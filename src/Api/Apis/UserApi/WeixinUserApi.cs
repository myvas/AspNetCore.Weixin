using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// User management
/// </summary>
public class WeixinUserApi : WeixinSecureApiClient, IWeixinUserApi
{
    public WeixinUserApi(IOptions<WeixinOptions> optionsAccessor, IWeixinAccessTokenApi tokenProvider) : base(optionsAccessor, tokenProvider)
    {
    }

    /// <summary>
    /// 获取微信用户资料。测试账号每日调用限制50万次。
    /// </summary>
    /// <param name="openId">微信用户标识。在同一公众号中，一个微信号（用户）每次订阅后得到的OpenId都是唯一的。</param>
    /// <param name="lang"><see cref="WeixinLanguage"/></param>
    /// <returns>微信用户资料</returns>
    /// <remarks>
    /// 错误时微信会返回错误码等信息，JSON数据包示例如下（该示例为AppID无效错误）:
    /// <code>
    /// {"errcode":40013,"errmsg":"invalid appid"}
    /// </code>
    /// <code>
    /// {"errcode":45009,"errmsg":"reach max api daily quota limit rid: 67dfb68b-1ea80c4e-408a6623"}
    /// </code>
    /// </remarks>
    /// <seealso href="https://developers.weixin.qq.com/doc/offiaccount/User_Management/Get_users_basic_information_UnionID.html#UinonId">Tencent official document: user/info</seealso>
    public async Task<WeixinUserInfoJson> Info(string openId, string lang = "zh_CN", CancellationToken cancellationToken = default)
    {
        var accessToken = await GetTokenAsync(cancellationToken);

        var pathAndQuery = "/cgi-bin/user/info?access_token={0}&openid={1}&lang={2}";
        pathAndQuery = string.Format(pathAndQuery, accessToken, openId, lang);
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        return await GetFromJsonAsync<WeixinUserInfoJson>(url, cancellationToken);
    }

    public Task<WeixinUserInfoJson> Info(string openId, WeixinLanguage lang, CancellationToken cancellationToken = default)
        => Info(openId, lang.Code, cancellationToken);

    /// <summary>
    /// 获取一个批次的订阅者的OpenId。测试账号每日调用限制500次，每次最多拉取1万个。
    /// </summary>
    /// <param name="nextOpenId">上一批列表的最后一个OPENID，不填则默认从头开始拉取</param>
    /// <remarks>一次拉取调用最多拉取10000个关注者的OpenID，可以通过多次拉取的方式来满足需求。</remarks>
    /// <seealso href="https://developers.weixin.qq.com/doc/offiaccount/User_Management/Getting_a_User_List.html">Tencent official document: user/get</seealso>
    public async Task<WeixinUserGetJson> Get(string nextOpenId, CancellationToken cancellationToken = default)
    {
        var accessToken = await GetTokenAsync(cancellationToken);

        var pathAndQuery = "/cgi-bin/user/get?access_token={0}&next_openid={1}";
        pathAndQuery = string.Format(pathAndQuery, accessToken, nextOpenId);
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);
        return await GetFromJsonAsync<WeixinUserGetJson>(url, cancellationToken);
    }

    /// <summary>
    /// 获取所有订阅者的OpenId
    /// </summary>
    public async Task<List<string>> GetAllOpenIds(CancellationToken cancellationToken = default)
    {
        List<string> openids = [];

        string nextOpenId = "";
        do
        {
            WeixinUserGetJson followerResult = await Get(nextOpenId, cancellationToken);
            if (!followerResult.Succeeded) break;
            int count = followerResult.count;
            if (count < 1) break;
            openids.AddRange(followerResult.data.openid);
            nextOpenId = followerResult.next_openid;
            if (openids.Count >= followerResult.total)
            {
                break;
            }
        } while (!string.IsNullOrEmpty(nextOpenId));

        return openids;
    }

    /// <summary>
    /// 获取所有订阅者的资料
    /// </summary>
    public async Task<List<WeixinUserInfoJson>> GetAllUserInfo(CancellationToken cancellationToken = default)
    {
        List<WeixinUserInfoJson> subscribers = [];

        List<string> openids = await GetAllOpenIds(cancellationToken);
        foreach (string openid in openids)
        {
            try
            {
                WeixinUserInfoJson userInfo = await Info(openid, cancellationToken: cancellationToken);
                if (userInfo?.Succeeded ?? false)
                {
                    subscribers.Add(userInfo);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        return subscribers;
    }
}
