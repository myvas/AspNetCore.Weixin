using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class VideoMessageReceivedEventArgs : EventArgs
    {
        public VideoMessageReceivedXml Data { get; set; }
    }
}
