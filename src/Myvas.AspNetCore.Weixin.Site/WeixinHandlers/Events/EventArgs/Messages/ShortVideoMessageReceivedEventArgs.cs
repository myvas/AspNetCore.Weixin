using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 小视频消息
    /// </summary>
    public class ShortVideoMessageReceivedEventArgs : EventArgs
    {
        public ShortVideoMessageReceivedXml Data { get; set; }
    }
}
