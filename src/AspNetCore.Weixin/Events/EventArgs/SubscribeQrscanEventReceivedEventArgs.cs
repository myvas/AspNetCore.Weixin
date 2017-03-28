using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 用户扫描带参数（场景值）二维码（扫描前未关注）
    /// </summary>
    public class SubscribeQrscanEventReceivedEventArgs : EventReceivedEventArgs
    {
        /// <summary>
        /// qrscene_为前缀，后面为二维码的参数值
        /// </summary>
        public string EventKey { get;set;}
        public string Ticket { get; set; }
    }
}
