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
    [XmlRoot("xml", Namespace = "")]
    public class VideoMessageReceivedEventArgs : MessageReceivedEventArgs
    {
        /// <summary>
        /// 视频消息
        /// </summary>
        public string MediaId { get; set; }
        public string ThumbMediaId { get; set; }

    }
}
