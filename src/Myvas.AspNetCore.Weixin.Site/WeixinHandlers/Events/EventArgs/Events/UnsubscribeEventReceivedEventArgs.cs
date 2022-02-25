using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 退订事件
    /// </summary>
    public class UnsubscribeEventReceivedEventArgs : EventArgs
    {
        public UnsubscribeEventReceivedXml Data { get; set; }
    }
}
