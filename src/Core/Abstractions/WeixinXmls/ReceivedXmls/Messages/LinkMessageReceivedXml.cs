using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到链接消息
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class LinkMessageReceivedXml : MessageReceivedXml
{
    /// <summary>
    /// 消息标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 消息描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 消息链接
    /// </summary>
    public string Url { get; set; }

}
