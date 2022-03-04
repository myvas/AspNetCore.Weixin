﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 客服接口
    /// </summary>
    public class CustomerSupportApi : WeixinApiClient
    {
        private const string WeixinApiUrlPattern = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";

        public CustomerSupportApi(HttpClient client) : base(client)
        {
        }

        /// <summary>
        /// 发送文本信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<WeixinErrorJson> SendText(string accessToken, string openId, string content)
        {
            var data = new
            {
                touser = openId,
                msgtype = "text",
                text = new
                {
                    content = content
                }
            };
            return await PostAsJsonAsync<object, WeixinErrorJson>(accessToken, WeixinApiUrlPattern, data);
        }

        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public async Task<WeixinErrorJson> SendImage(string accessToken, string openId, string mediaId)
        {
            var data = new
            {
                touser = openId,
                msgtype = "image",
                image = new
                {
                    media_id = mediaId
                }
            };
            return await PostAsJsonAsync<object, WeixinErrorJson>(accessToken, WeixinApiUrlPattern, data);
        }

        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public async Task<WeixinErrorJson> SendVoice(string accessToken, string openId, string mediaId)
        {
            var data = new
            {
                touser = openId,
                msgtype = "voice",
                voice = new
                {
                    media_id = mediaId
                }
            };
            return await PostAsJsonAsync<object, WeixinErrorJson>(accessToken, WeixinApiUrlPattern, data);
        }

        /// <summary>
        /// 发送视频消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        /// <param name="thumbMediaId"></param>
        /// <returns></returns>
        public async Task<WeixinErrorJson> SendVideo(string accessToken, string openId, string mediaId, string thumbMediaId)
        {
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
            return await PostAsJsonAsync<object, WeixinErrorJson>(accessToken, WeixinApiUrlPattern, data);
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
        public async Task<WeixinErrorJson> SendMusic(string accessToken, string openId, string title, string description,
                                    string musicUrl, string hqMusicUrl, string thumbMediaId)
        {
            var data = new
            {
                touser = openId,
                msgtype = "music",
                music = new
                {
                    title = title,
                    description = description,
                    musicurl = musicUrl,
                    hqmusicurl = hqMusicUrl,
                    thumb_media_id = thumbMediaId
                }
            };
            return await PostAsJsonAsync<object, WeixinErrorJson>(accessToken, WeixinApiUrlPattern, data);
        }

        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <param name="articles"></param>
        /// <returns></returns>
        public async Task<WeixinErrorJson> SendNews(string accessToken, string openId, List<Article> articles)
        {
            var data = new
            {
                touser = openId,
                msgtype = "news",
                news = new
                {
                    articles = articles.Select(z => new
                    {
                        title = z.Title,
                        description = z.Description,
                        url = z.Url,
                        picurl = z.PicUrl//图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80
                    }).ToList()
                }
            };
            return await PostAsJsonAsync<object, WeixinErrorJson>(accessToken, WeixinApiUrlPattern, data);
        }
    }
}
