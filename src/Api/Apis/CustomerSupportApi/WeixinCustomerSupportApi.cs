using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 客服接口
/// </summary>
public class WeixinCustomerSupportApi : WeixinSecureApiClient, IWeixinCustomerSupportApi
{
    public WeixinCustomerSupportApi(IOptions<WeixinOptions> optionsAccessor, IWeixinAccessTokenApi tokenProvider) : base(optionsAccessor, tokenProvider)
    {
    }

    /// <summary>
    /// 发送文本信息
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="openId"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public async Task<WeixinErrorJson> SendText(string openId, string content)
    {
        var pathAndQuery = "/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        var data = new
        {
            touser = openId,
            msgtype = "text",
            text = new
            {
                content
            }
        };
        return await SecurePostAsJsonAsync<object, WeixinErrorJson>(url, data);
    }

    /// <summary>
    /// 发送图片消息
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="openId"></param>
    /// <param name="mediaId"></param>
    /// <returns></returns>
    public async Task<WeixinErrorJson> SendImage(string openId, string mediaId)
    {
        var pathAndQuery = "/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        var data = new
        {
            touser = openId,
            msgtype = "image",
            image = new
            {
                media_id = mediaId
            }
        };
        return await SecurePostAsJsonAsync<object, WeixinErrorJson>(url, data);
    }

    /// <summary>
    /// 发送语音消息
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="openId"></param>
    /// <param name="mediaId"></param>
    /// <returns></returns>
    public async Task<WeixinErrorJson> SendVoice(string openId, string mediaId)
    {
        var pathAndQuery = "/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        var data = new
        {
            touser = openId,
            msgtype = "voice",
            voice = new
            {
                media_id = mediaId
            }
        };
        return await SecurePostAsJsonAsync<object, WeixinErrorJson>(url, data);
    }

    /// <summary>
    /// 发送视频消息
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="openId"></param>
    /// <param name="mediaId"></param>
    /// <param name="thumbMediaId"></param>
    /// <returns></returns>
    public async Task<WeixinErrorJson> SendVideo(string openId, string mediaId, string thumbMediaId)
    {
        var pathAndQuery = "/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        var data = new
        {
            touser = openId,
            msgtype = "video",
            video = new
            {
                media_id = mediaId,
                thumb_media_id = thumbMediaId
            }
        };
        return await SecurePostAsJsonAsync<object, WeixinErrorJson>(url, data);
    }

    /// <summary>
    /// 发送音乐消息
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="openId"></param>
    /// <param name="title">音乐标题（非必须）</param>
    /// <param name="description">音乐描述（非必须）</param>
    /// <param name="musicUrl">音乐链接</param>
    /// <param name="hqMusicUrl">高品质音乐链接，wifi环境优先使用该链接播放音乐</param>
    /// <param name="thumbMediaId">视频缩略图的媒体ID</param>
    /// <returns></returns>
    public async Task<WeixinErrorJson> SendMusic(string openId, string title, string description,
                                string musicUrl, string hqMusicUrl, string thumbMediaId)
    {
        var pathAndQuery = "/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        var data = new
        {
            touser = openId,
            msgtype = "music",
            music = new
            {
                title,
                description,
                musicurl = musicUrl,
                hqmusicurl = hqMusicUrl,
                thumb_media_id = thumbMediaId
            }
        };
        return await SecurePostAsJsonAsync<object, WeixinErrorJson>(url, data);
    }

    /// <summary>
    /// 发送图文消息
    /// </summary>
    /// <returns></returns>
    public async Task<WeixinErrorJson> SendNews(WeixinCustomerServiceMessageNews news, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);


        var json = news.ToJson();
        return await SecurePostContentAsJsonAsync<WeixinErrorJson>(url, new StringContent(json), cancellationToken);
    }

    public async Task<WeixinErrorJson> SendNews(string destOpenId, IList<WeixinCustomerServiceMessageNewsArticle> articles, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/message/custom/send?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        var news = new WeixinCustomerServiceMessageNews
        {
            ToUser = destOpenId
        };
        articles.ToList().ForEach(x => news.AddArticle(x));
        var json = news.ToJson();
        return await SecurePostContentAsJsonAsync<WeixinErrorJson>(url, new StringContent(json), cancellationToken);
    }
}
