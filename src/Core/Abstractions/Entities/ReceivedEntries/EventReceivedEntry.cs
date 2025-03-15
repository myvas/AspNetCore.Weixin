using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 收到事件（非普通消息）
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class EventReceivedEntry : ReceivedEntry
{
    /// <summary>
    /// 事件类型
    /// </summary>
    [XmlElement("Event", Namespace = "")]
    public string Event { get; set; }

    /// <summary>
    /// Gets the <see cref="RequestEventType"/> parsed from the <see cref="Event"/>.
    /// </summary>
    /// <returns>The <see cref="RequestEventType"/>.</returns>
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

    /// <summary>
    /// Sets the <see cref="Event"/> cast from the <see cref="RequestEventType"/> .
    /// </summary>
    /// <param name="value">The <see cref="RequestEventType"/>.</param>
    public string SetEventEnum(RequestEventType value)
    {
        Event = value.ToString();
        return Event;
    }
}
