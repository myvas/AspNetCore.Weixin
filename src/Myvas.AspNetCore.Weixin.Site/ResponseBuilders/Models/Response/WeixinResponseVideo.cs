using System;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{


    /// <summary>
    /// 需要预先上传多媒体文件到微信服务器，只支持认证服务号。
    /// </summary>
    [XmlRoot("xml", Namespace = "")]
    public class WeixinResponseVideo : WeixinResponse, IWeixinResponse
    {
        public Video Video { get; set; }

        public WeixinResponseVideo()
        {
            MsgType = ResponseMsgType.video;
            Video = new Video();
        }
    }
}
