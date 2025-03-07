using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 收到链接消息
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class LinkMessageReceivedEntry : MessageReceivedEntry
{
    /// <summary>
    /// 消息标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 消息描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 消息链接
    /// </summary>
    public string Url { get; set; }
}
