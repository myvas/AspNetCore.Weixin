using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    [XmlRoot("xml", Namespace = "")]
    public class WeixinResponseText : WeixinResponse, IWeixinResponse
    {
        public WeixinResponseText()
        {
            MsgType = ResponseMsgType.text;
        }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string Content { get; set; }
    }
}
