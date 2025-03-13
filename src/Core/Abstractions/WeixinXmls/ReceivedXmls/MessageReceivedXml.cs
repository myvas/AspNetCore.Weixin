using System;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到普通消息（除事件外的消息）
/// </summary>
public class MessageReceivedXml : ReceivedXml
{
    public Int64 MsgId { get; set; }
}
