using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Weixin
{
    public static class Message
    {
        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openID"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<WeixinErrorJson> SendText(string accessToken, string openID, string content)
        {
            var data = new
            {
                touser = openID,
                msgtype = "text",
                text = new
                {
                    content = content
                }
            };
            return await MessageSend.Send(accessToken,
                "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", 
                data);
        }

        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openID"></param>
        /// <param name="mediaID"></param>
        /// <returns></returns>
        public static async Task<WeixinErrorJson> SendImage(string accessToken, string openID, string mediaID)
        {
            var data = new
            {
                touser = openID,
                msgtype = "image",
                image = new
                {
                    media_id = mediaID
                }
            };
            return await MessageSend.Send(accessToken, "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", data);
        }

        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openID"></param>
        /// <param name="mediaID"></param>
        /// <returns></returns>
        public static async Task<WeixinErrorJson> SendVoice(string accessToken, string openID, string mediaID)
        {
            var data = new
            {
                touser = openID,
                msgtype = "voice",
                voice = new
                {
                    media_id = mediaID
                }
            };
            return await MessageSend.Send(accessToken, "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", data);
        }

        /// <summary>
        /// 发送视频消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openID"></param>
        /// <param name="mediaID"></param>
        /// <param name="thumbMediaID"></param>
        /// <returns></returns>
        public static async Task<WeixinErrorJson> SendVideo(string accessToken, string openID, string mediaID, string thumbMediaID)
        {
            var data = new
            {
                touser = openID,
                msgtype = "video",
                video = new
                {
                    media_id = mediaID,
                    thumb_media_id = thumbMediaID
                }
            };
            return await MessageSend.Send(accessToken, "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", data);
        }

        /// <summary>
        /// 发送音乐消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openID"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="musicUrl"></param>
        /// <param name="hqMusicUrl"></param>
        /// <param name="thumbMediaID"></param>
        /// <returns></returns>
        public static async Task<WeixinErrorJson> SendMusic(string accessToken, string openID, string title, string description,
                                    string musicUrl, string hqMusicUrl, string thumbMediaID)
        {
            var data = new
            {
                touser = openID,
                msgtype = "music",
                music = new
                {
                    title = title,
                    description = description,
                    musicurl = musicUrl,
                    hqmusicurl = hqMusicUrl,
                    thumb_media_id = thumbMediaID
                }
            };
            return await MessageSend.Send(accessToken, "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", data);
        }

        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openID"></param>
        /// <param name="articles"></param>
        /// <returns></returns>
        public static async Task<WeixinErrorJson> SendNews(string accessToken, string openID, List<Article> articles)
        {
            var data = new
            {
                touser = openID,
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
            return await MessageSend.Send(accessToken, "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", data);
        }


    }


}
