using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 收到事件（非普通消息）
    /// </summary>
    public class EventReceivedEventArgs : ReceivedEventArgs
    {

        public WeixinEvent Event { get; set; }
    }
}
