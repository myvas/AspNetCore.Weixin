using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到点击菜单跳转链接事件
    /// </summary>
    [XmlRoot("xml", Namespace = "")]
    public class ViewMenuEventReceivedXml : EventReceivedXml
    {
        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应。另有别名：<see cref="MenuItemKey"/>
        /// </summary>
        public string EventKey { get; set; }
        [XmlIgnore]
        /// <summary>
        /// 跳转链接。这是<see cref="EventKey"/>的别名。
        /// </summary>
        public string Url { get => EventKey; set => EventKey = value; }
    }
}
