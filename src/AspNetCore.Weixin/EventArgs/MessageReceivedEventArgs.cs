using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到普通消息（除事件外的消息）
    /// </summary>
    public class MessageReceivedEventArgs : ReceivedEventArgs
    {
        public Int64 MsgId { get; set; }
    }
}
