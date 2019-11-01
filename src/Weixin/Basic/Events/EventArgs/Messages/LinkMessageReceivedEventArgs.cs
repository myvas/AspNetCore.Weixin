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
    [XmlRoot("xml", Namespace = "")]
    public class LinkMessageReceivedEventArgs : MessageReceivedEventArgs
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

    }
}
