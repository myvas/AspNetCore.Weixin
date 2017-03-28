using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    public class ResponseMessageText : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Text; }
        }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string Content { get; set; }
    }

    public class ResponseMessageNews : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.News; }
        }

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
        public List<Article> Articles { get; set; }

        public ResponseMessageNews()
        {
            Articles = new List<Article>();
        }
    }

    public class ResponseMessageMusic : ResponseMessageBase, IResponseMessageBase
    {
        public override ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Music; }
        }

        public Music Music { get; set; }
        public string ThumbMediaId { get; set; }

        public ResponseMessageMusic()
        {
            Music = new Music();
        }
    }


    public class ResponseMessageImage : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Image; }
        }

        public Image Image { get; set; }

        public ResponseMessageImage()
        {
            Image = new Image();
        }
    }


    /// <summary>
    /// 需要预先上传多媒体文件到微信服务器，只支持认证服务号。
    /// </summary>
    public class ResponseMessageVoice : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Voice; }
        }

        public Voice Voice { get; set; }

        public ResponseMessageVoice()
        {
            Voice = new Voice();
        }
    }


    /// <summary>
    /// 需要预先上传多媒体文件到微信服务器，只支持认证服务号。
    /// </summary>
    public class ResponseMessageVideo : ResponseMessageBase, IResponseMessageBase
    {
        new public virtual ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Video; }
        }

        public Video Video { get; set; }

        public ResponseMessageVideo()
        {
            Video = new Video();
        }
    }




}
