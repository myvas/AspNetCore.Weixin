using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 收到图片消息
    /// </summary>
    public class ImageMessageReceivedEventArgs : MessageReceivedEventArgs
    {
        public string PicUrl { get; set; }
        public string MediaId { get; set; }

    }
}
