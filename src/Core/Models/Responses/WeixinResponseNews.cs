using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    [XmlRoot("xml", Namespace = "")]
    public class WeixinResponseNews : WeixinResponse, IWeixinResponse
    {
        public int ArticleCount
        {
            get { return (Articles ?? new List<WeixinResponseNewsArticle>()).Count; }
            set
            {
                //这里开放set只为了逆向从Response的Xml转成实体的操作一致性，没有实际意义。
            }
        }

        /// <summary>
        /// 文章列表，微信客户端只能输出前10条（可能未来数字会有变化，出于视觉效果考虑，建议控制在8条以内）
        /// </summary>
        [XmlArrayItem("item")]
        public List<WeixinResponseNewsArticle> Articles { get; set; }

        public WeixinResponseNews()
        {
            MsgType = ResponseMsgType.news;
            Articles = new List<WeixinResponseNewsArticle>();
        }
    }

    public class WeixinResponseNewsArticle
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PicUrl { get; set; }
        public string Url { get; set; }
    }
}
