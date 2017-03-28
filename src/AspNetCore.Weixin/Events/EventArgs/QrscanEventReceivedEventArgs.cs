using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 用户扫描带参数（场景值）二维码（扫描前已关注）
    /// </summary>
    public class QrscanEventReceivedEventArgs : EventReceivedEventArgs
    {
        /// <summary>
        /// 创建二维码时的二维码scene_id（一个32位无符号整数）
        /// </summary>
        public string SceneId { get; set; }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }
}
