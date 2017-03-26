using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到语音消息
    /// </summary>
    public class VoiceMessageReceivedEventArgs : MessageReceivedEventArgs
    {
        public string MediaId { get; set; }
        public string Format { get; set; }

    }
}
