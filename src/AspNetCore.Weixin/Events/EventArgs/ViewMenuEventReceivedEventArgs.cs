using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 收到菜单事件
    /// </summary>
    public class ViewMenuEventReceivedEventArgs : EventReceivedEventArgs
    {
        /// <summary>
        /// 设置的跳转URL
        /// <example>www.demo.com</example>
        /// </summary>
        public string Url { get; set; }
    }
}
