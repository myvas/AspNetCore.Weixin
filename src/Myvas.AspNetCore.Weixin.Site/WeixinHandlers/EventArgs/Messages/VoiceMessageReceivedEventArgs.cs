using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到语音消息
    /// </summary>
    public class VoiceMessageReceivedEventArgs : EventArgs
    {
        public VoiceMessageReceivedXml Data { get; set; }
    }
}
