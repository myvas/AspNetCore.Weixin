using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 订阅事件。或，用户扫描带参数（场景值）二维码（扫描前未关注）。
    /// </summary>
    public class SubscribeEventReceivedEventArgs : EventArgs
    {
        public SubscribeEventReceivedXml Data { get; set; }
    }
}
