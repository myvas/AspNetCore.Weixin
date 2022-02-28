using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Entities;

/// <summary>
/// 收到文本消息
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class TextMessageReceivedEntity : MessageReceivedEntity
{
    /// <summary>
    /// 文本消息内容
    /// </summary>
    public string Content { get; set; }
}
