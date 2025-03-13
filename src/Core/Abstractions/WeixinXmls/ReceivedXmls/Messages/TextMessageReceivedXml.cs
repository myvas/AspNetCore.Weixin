using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到文本消息
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class TextMessageReceivedXml : MessageReceivedXml
{
    public string Content { get; set; }
}
