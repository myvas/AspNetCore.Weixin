using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Entities;

/// <summary>
/// 收到点击菜单跳转链接事件
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class ViewMenuEventReceivedEntity : EventReceivedEntity
{
    /// <summary>
    /// 事件KEY值，与自定义菜单接口中KEY值对应
    /// </summary>
    public string EventKey { get; set; }
}
