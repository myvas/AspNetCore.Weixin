using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到菜单事件
    /// </summary>
    public class ClickMenuEventReceivedEventArgs : EventReceivedEventArgs
    {
        /// <summary>
        /// 与自定义菜单接口中KEY值对应
        /// </summary>
        public string MenuItemKey { get; set; }
    }
}
