using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    public class VideoMessageReceivedEventArgs : MessageReceivedEventArgs
    {
        /// <summary>
        /// 视频消息
        /// </summary>
        public string MediaId { get; set; }
        public string ThumbMediaId { get; set; }

    }
}
