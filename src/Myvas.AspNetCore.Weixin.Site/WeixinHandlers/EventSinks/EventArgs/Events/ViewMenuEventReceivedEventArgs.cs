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
    public class ViewMenuEventReceivedEventArgs : EventArgs
    {
        public ViewMenuEventReceivedXml Data { get; set; }
    }
}
