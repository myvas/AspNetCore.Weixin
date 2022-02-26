using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 上报地理位置事件
    /// </summary>
    public class LocationEventReceivedEventArgs : EventArgs
    {
        public LocationEventReceivedXml Data { get; set; }
    }
}
