using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到普通消息（除事件外的消息）
    /// </summary>
    [XmlRoot("xml", Namespace = "")]
    public class MessageReceivedEventArgs : ReceivedEventArgs
    {
        public Int64 MsgId { get; set; }
    }
}
