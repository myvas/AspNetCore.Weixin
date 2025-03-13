using System;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到普通消息（除事件外的消息）
/// </summary>
/// <remarks>https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Receiving_standard_messages.html</remarks>
[XmlRoot("xml", Namespace = "")]
public class MessageReceivedXml : ReceivedXml
{
    /// <summary>
    /// 消息id，64位整型
    /// </summary>
    public Int64 MsgId { get; set; }
}
