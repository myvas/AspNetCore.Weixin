using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到图片消息
    /// </summary>
    public class ImageMessageReceivedEventArgs : EventArgs
    {
        public ImageMessageReceivedXml Data { get; set; }
    }
}
