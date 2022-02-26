using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到点击菜单拉取消息事件
    /// </summary>
    public class ClickMenuEventReceivedEventArgs : EventArgs
    {
        public ClickMenuEventReceivedXml Data { get; set; }
    }
}
