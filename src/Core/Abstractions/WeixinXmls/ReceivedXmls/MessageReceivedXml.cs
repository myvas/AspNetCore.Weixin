using System;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到普通消息（除事件外的消息）
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class MessageReceivedXml : ReceivedXml
{
    public Int64 MsgId { get; set; }
}
