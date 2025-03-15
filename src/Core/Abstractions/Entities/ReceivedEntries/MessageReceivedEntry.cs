using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 收到普通消息（除事件外的消息）
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class MessageReceivedEntry : ReceivedEntry
{
    /// <summary>
    /// 消息id，64位整型
    /// </summary>
    public Int64 MsgId { get; set; }
}
