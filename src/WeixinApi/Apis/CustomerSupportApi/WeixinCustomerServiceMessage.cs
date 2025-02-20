using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinCustomerServiceMessage
    {
        [JsonProperty("touser")]
        public string ToUser { get; set; }

        [JsonProperty("msgtype")]
        public string MsgType { get; set; }

    }

    public class WeixinCustomerServiceMessageText : WeixinCustomerServiceMessage
    {
        public string Content { get; set; }
    }

    /// <summary>
    /// 图片资源
    /// </summary>
    public class WeixinCustomerServiceMessageImage : WeixinCustomerServiceMessage
    {
        /// <summary>
        /// 资源号
        /// </summary>
        public string MediaId { get; set; }
    }
    /// <summary>
    /// 声音资源
    /// </summary>
    public class WeixinCustomerServiceMessageVoice : WeixinCustomerServiceMessage
    {
        /// <summary>
        /// 资源号
        /// </summary>
        public string MediaId { get; set; }

    }
    /// <summary>
    /// 视频资源
    /// </summary>
    public class WeixinCustomerServiceMessageVideo : WeixinCustomerServiceMessage
    {
        /// <summary>
        /// 资源号
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

    }
    /// <summary>
    /// 音乐资源
    /// </summary>
    public class WeixinCustomerServiceMessageMusic : WeixinCustomerServiceMessage
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 音乐地址
        /// </summary>
        public string MusicUrl { get; set; }
        /// <summary>
        /// 音乐地址（高质量版）
        /// </summary>
        public string HQMusicUrl { get; set; }
        ///// <summary>
        ///// 缩略图的媒体id，通过上传多媒体文件，得到的id
        ///// 官方API上有，但是加入的话会出错
        ///// </summary>
        //public string ThumbMediaId { get; set; }

    }

    public partial class WeixinCustomerServiceMessageNews : WeixinCustomerServiceMessage
    {
        public WeixinCustomerServiceMessageNews()
        {
            MsgType = WeixinCustomerServiceMessageTypes.News;
        }

        public IReadOnlyList<WeixinCustomerServiceMessageNewsArticle> Articles { get { return (IReadOnlyList<WeixinCustomerServiceMessageNewsArticle>)_articles; } }
        private readonly IList<WeixinCustomerServiceMessageNewsArticle> _articles = new List<WeixinCustomerServiceMessageNewsArticle>();

        public string ToJson()
        {
            var data = new
            {
                touser = ToUser,
                msgtype = MsgType,
                news = new
                {
                    articles = Articles
                }
            };
            return JsonConvert.SerializeObject(data);
        }

        public void AddArticle(WeixinCustomerServiceMessageNewsArticle article)
        {
            _articles.Add(article);
        }
    }

    /// <summary>
    /// 图文消息内容
    /// </summary>
    public class WeixinCustomerServiceMessageNewsArticle
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图片地址。支持JPG、PNG格式，较好的效果为大图360*200，小图200*200
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 跳转链接地址
        /// </summary>
        public string Url { get; set; }
    }
}
