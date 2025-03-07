﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 用户扫描带参数（场景值）二维码（扫描前已关注）
    /// </summary>
    [XmlRoot("xml", Namespace = "")]
    public class QrscanEventReceivedXml : EventReceivedXml
    {
        /// <summary>
        /// 事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id（整数或字符串）
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 场景ID
        /// </summary>
        [XmlIgnore]
        public string Scene { get => EventKey; set => EventKey = value; }

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }
}
