using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到事件（非普通消息）
/// </summary>
/// <remarks>https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Receiving_event_pushes.html</remarks>
[XmlRoot("xml", Namespace = "")]
public class EventReceivedXml : ReceivedXml
{
    /// <summary>
    /// 事件类型
    /// </summary>
    [XmlElement("Event")]
    public string Event { get; set; }
}
