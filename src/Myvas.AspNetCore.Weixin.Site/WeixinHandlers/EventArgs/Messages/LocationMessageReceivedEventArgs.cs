using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到地理位置消息
    /// </summary>
    public class LocationMessageReceivedEventArgs : EventArgs
    {
        public LocationMessageReceivedXml Data { get; set; }
    }
}
