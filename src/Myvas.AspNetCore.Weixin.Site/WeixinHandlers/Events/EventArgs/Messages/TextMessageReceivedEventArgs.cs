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
    public class TextMessageReceivedEventArgs : EventArgs
    {
        public TextMessageReceivedXml Data { get; set; }
    }
}
