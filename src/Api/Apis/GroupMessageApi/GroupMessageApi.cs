﻿using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 高级群发接口
/// </summary>
public class GroupMessageApi : SecureWeixinApiClient, IGroupMessageApi
{
    public GroupMessageApi(IOptions<WeixinOptions> optionsAccessor, IWeixinAccessTokenApi tokenProvider) : base(optionsAccessor, tokenProvider)
    {
    }

    /// <summary>
    /// 根据分组进行群发
    /// 
    /// 请注意：
    /// 1、该接口暂时仅提供给已微信认证的服务号
    /// 2、虽然开发者使用高级群发接口的每日调用限制为100次，但是用户每月只能接收4条，请小心测试
    /// 3、无论在公众平台网站上，还是使用接口群发，用户每月只能接收4条群发消息，多于4条的群发将对该用户发送失败。
    /// 
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="groupId">群发到的分组的group_id</param>
    /// <param name="mediaId">用于群发的消息的media_id</param>
    /// <returns></returns>
    public async Task<SendResult> SendGroupMessageByGroupId(string groupId, string mediaId)
    {
        var pathAndQuery = "/cgi-bin/message/mass/sendall?access_token={0}";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        var data = new
        {
            filter = new
            {
                group_id = groupId
            },
            mpnews = new
            {
                media_id = mediaId
            },
            msgtype = "mpnews"
        };
        return await SecurePostAsJsonAsync<object, SendResult>(url, data);
    }

    /// <summary>
    /// 根据OpenId进行群发
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="mediaId">用于群发的消息的media_id</param>
    /// <param name="openIds">openId字符串数组</param>
    /// <returns></returns>
    public async Task<SendResult> SendGroupMessageByOpenId(string mediaId, params string[] openIds)
    {
        var pathAndQuery = "/cgi-bin/message/mass/send?access_token={0}";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        var data = new
        {
            touser = openIds,
            mpnews = new
            {
                media_id = mediaId
            },
            msgtype = "mpnews"
        };
        return await SecurePostAsJsonAsync<object, SendResult>(url, data);
    }

    /// <summary>
    /// 删除群发消息
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="mediaId">发送出去的消息ID</param>
    /// <returns></returns>
    public async Task<WeixinErrorJson> DeleteSendMessage(string mediaId)
    {
        //官方API地址为https://api.weixin.qq.com//cgi-bin/message/mass/delete?access_token={0}，应该是多了一个/
        var pathAndQuery = "/cgi-bin/message/mass/delete?access_token={0}";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        var data = new
        {
            msgid = mediaId
        };
        return await SecurePostAsJsonAsync<object, WeixinErrorJson>(url, data);
    }
}