﻿using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到点击菜单拉取消息事件
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class ClickMenuEventReceivedXml : EventReceivedXml
{
    /// <summary>
    /// 事件KEY值，与自定义菜单接口中KEY值对应。另有别名：<see cref="MenuItemKey"/>
    /// </summary>
    [XmlElement("EventKey")]
    public string EventKey { get; set; }
}
