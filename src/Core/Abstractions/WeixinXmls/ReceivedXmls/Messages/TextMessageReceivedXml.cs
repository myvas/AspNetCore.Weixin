using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 文本消息
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class TextMessageReceivedXml : MessageReceivedXml
{
    /// <summary>
    /// 文本消息内容
    /// </summary>
    public string Content { get; set; }
}
