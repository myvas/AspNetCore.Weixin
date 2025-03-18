using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 收到点击菜单拉取消息事件
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class ClickMenuEventReceivedEntry : EventReceivedEntry
{
    /// <summary>
    /// 事件KEY值，设置的跳转URL
    /// </summary>
    public string EventKey { get; set; }
}
