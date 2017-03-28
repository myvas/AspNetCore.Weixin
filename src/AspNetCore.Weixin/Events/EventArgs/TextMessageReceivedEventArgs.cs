using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 收到文本消息
    /// </summary>
    public class TextMessageReceivedEventArgs : MessageReceivedEventArgs
    {
        public string Content { get; set; }

    }
}
