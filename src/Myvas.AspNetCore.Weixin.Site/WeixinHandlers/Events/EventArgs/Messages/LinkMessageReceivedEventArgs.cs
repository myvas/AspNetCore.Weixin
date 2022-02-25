using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到链接消息
    /// </summary>
    public class LinkMessageReceivedEventArgs : EventArgs
    {
        public LinkMessageReceivedXml Data { get; set; }
    }
}
