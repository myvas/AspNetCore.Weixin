using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 对话上下文被删除时触发事件的事件数据
    /// </summary>
    public class WeixinContextEventArgs : EventArgs
    {
        /// <summary>
        /// 该用户的OpenId
        /// </summary>
        public string OpenId
        {
            get
            {
                return MessageContext.UserName;
            }
        }
        /// <summary>
        /// 最后一次响应时间
        /// </summary>
        public DateTime LastActiveTime
        {
            get
            {
                return MessageContext.LastActiveTime;
            }
        }
        /// <summary>
        /// 上下文对象
        /// </summary>
        public IMessageContext MessageContext { get; set; }

        public WeixinContextEventArgs(IMessageContext messageContext)
        {
            MessageContext = messageContext;
        }
    }
}
