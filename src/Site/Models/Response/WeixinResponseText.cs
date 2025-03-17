using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

[XmlRoot("xml", Namespace = "")]
public class WeixinResponseText : WeixinResponse, IWeixinResponseMessage
{
    /// <summary>
    /// 文本内容
    /// </summary>
    public string Content { get; set; }

    public WeixinResponseText()
    {
        MsgType = ResponseMsgType.text;
    }

    public WeixinResponseText(string content) : this()
    {
        Content = content;
    }
}
