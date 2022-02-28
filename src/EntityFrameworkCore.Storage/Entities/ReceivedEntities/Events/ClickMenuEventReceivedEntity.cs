using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Entities;

/// <summary>
/// 收到点击菜单拉取消息事件
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class ClickMenuEventReceivedEntity : EventReceivedEntity
{
    /// <summary>
    /// 事件KEY值，设置的跳转URL
    /// </summary>
    public string EventKey { get; set; }
}
