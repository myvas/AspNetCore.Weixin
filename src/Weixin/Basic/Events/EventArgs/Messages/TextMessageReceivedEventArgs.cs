using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到文本消息
    /// </summary>
    [XmlRoot("xml", Namespace = "")]
    public class TextMessageReceivedEventArgs : MessageReceivedEventArgs
    {
        public string Content { get; set; }
    }
}
