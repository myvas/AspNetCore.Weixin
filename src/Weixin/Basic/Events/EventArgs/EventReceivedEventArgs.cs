using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到事件（非普通消息）
    /// </summary>
    [XmlRoot("xml", Namespace = "")]
    public class EventReceivedEventArgs : ReceivedEventArgs
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        [XmlElement("Event", Namespace = "")]
        public string EventValue { get; set; }
        [XmlIgnore]
        public ReceivedEventType Event
        {
            get
            {
                return (ReceivedEventType)Enum.Parse(typeof(ReceivedEventType), EventValue, true);
            }
            set
            {
                EventValue = value.ToString();
            }
        }
    }
}
