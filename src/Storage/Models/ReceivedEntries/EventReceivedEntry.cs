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
    /// 
    /// </summary>
    [XmlIgnore]
    [NotMapped]
    public RequestEventType EventEnum
    {
        get
        {
            return (RequestEventType)Enum.Parse(typeof(RequestEventType), Event, true);
        }
        set
        {
            Event = value.ToString();
        }
    }
}
