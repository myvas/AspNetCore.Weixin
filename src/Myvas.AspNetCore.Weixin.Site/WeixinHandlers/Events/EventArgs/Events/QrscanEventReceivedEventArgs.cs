using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 用户扫描带参数（场景值）二维码（扫描前已关注）
    /// </summary>
    public class QrscanEventReceivedEventArgs : EventArgs
    {
        public QrscanEventReceivedXml Data { get; set; }
    }
}
