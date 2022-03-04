using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    [XmlElement("Event", Namespace = "")]
    public string Event { get; set; }

    //[XmlIgnore]
    //[NotMapped]
    public RequestEventType GetEventEnum()
    {
        try
        {
            return (RequestEventType)Enum.Parse(typeof(RequestEventType), Event, true);
        }
        catch
        {
            return RequestEventType.Unknown;
        }
    }
}
