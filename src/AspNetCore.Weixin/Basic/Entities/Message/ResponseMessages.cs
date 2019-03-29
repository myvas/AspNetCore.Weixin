using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 将消息转发到客服，暂不支持将消息转发到指定客服!!!
    /// </summary>
    /// <remarks>ref to: https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1458557405 </remarks>
    [XmlRoot("xml", Namespace = "")]
    public class ResponseMessageTransferCustomerService : ResponseMessageBase, IResponseMessageBase
    {
        public ResponseMessageTransferCustomerService()
        {
            MsgType = ResponseMsgType.transfer_customer_service;
        }

        ///// <summary>
        ///// TODO:将消息转发到指定客服
        ///// </summary>
        //[XmlArrayItem("TransInfo")]
        //public List<KfAccountItem> TransInfo { get; set; }
    }

    [XmlRoot("xml", Namespace = "")]
    public class ResponseMessageText : ResponseMessageBase, IResponseMessageBase
    {
        public ResponseMessageText()
        {
            MsgType = ResponseMsgType.text;
        }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string Content { get; set; }
    }

    [XmlRoot("xml", Namespace = "")]
    public class ResponseMessageNews : ResponseMessageBase, IResponseMessageBase
    {
        public int ArticleCount
        {
            get { return (Articles ?? new List<Article>()).Count; }
            set
            {
                //这里开放set只为了逆向从Response的Xml转成实体的操作一致性，没有实际意义。
            }
        }

        /// <summary>
        /// 文章列表，微信客户端只能输出前10条（可能未来数字会有变化，出于视觉效果考虑，建议控制在8条以内）
        /// </summary>
        [XmlArrayItem("item")]
        public List<Article> Articles { get; set; }

        public ResponseMessageNews()
        {
            MsgType = ResponseMsgType.news;
            Articles = new List<Article>();
        }
    }

    [XmlRoot("xml", Namespace = "")]
    public class ResponseMessageMusic : ResponseMessageBase, IResponseMessageBase
    {
        public Music Music { get; set; }
        public string ThumbMediaId { get; set; }

        public ResponseMessageMusic()
        {
            MsgType = ResponseMsgType.music;
            Music = new Music();
        }
    }


    [XmlRoot("xml", Namespace = "")]
    public class ResponseMessageImage : ResponseMessageBase, IResponseMessageBase
    {
        public Image Image { get; set; }

        public ResponseMessageImage()
        {
            MsgType = ResponseMsgType.image;
            Image = new Image();
        }
    }


    /// <summary>
    /// 需要预先上传多媒体文件到微信服务器，只支持认证服务号。
    /// </summary>
    [XmlRoot("xml", Namespace = "")]
    public class ResponseMessageVoice : ResponseMessageBase, IResponseMessageBase
    {
        public Voice Voice { get; set; }

        public ResponseMessageVoice()
        {
            MsgType = ResponseMsgType.voice;
            Voice = new Voice();
        }
    }


    /// <summary>
    /// 需要预先上传多媒体文件到微信服务器，只支持认证服务号。
    /// </summary>
    [XmlRoot("xml", Namespace = "")]
    public class ResponseMessageVideo : ResponseMessageBase, IResponseMessageBase
    {
        public Video Video { get; set; }

        public ResponseMessageVideo()
        {
            MsgType = ResponseMsgType.video;
            Video = new Video();
        }
    }
}
