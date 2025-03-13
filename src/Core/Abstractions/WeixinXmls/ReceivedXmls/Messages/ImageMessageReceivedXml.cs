using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到图片消息
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class ImageMessageReceivedXml : MessageReceivedXml
{
    public string PicUrl { get; set; }
    public string MediaId { get; set; }

}
