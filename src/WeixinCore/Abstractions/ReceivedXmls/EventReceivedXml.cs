﻿using System;
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
    public class EventReceivedXml : ReceivedXml
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        [XmlElement("Event", Namespace = "")]
        public string EventValue { get; set; }
        [XmlIgnore]
        public RequestEventType Event
        {
            get
            {
                return (RequestEventType)Enum.Parse(typeof(RequestEventType), EventValue, true);
            }
            set
            {
                EventValue = value.ToString();
            }
        }
    }
}
