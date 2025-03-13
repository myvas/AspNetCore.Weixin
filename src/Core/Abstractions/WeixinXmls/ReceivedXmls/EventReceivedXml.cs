using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到事件（非普通消息）
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class EventReceivedXml : ReceivedXml
{
    /// <summary>
    /// 事件类型
    /// </summary>
    [XmlElement("Event")]
    public string Event { get; set; }
}
