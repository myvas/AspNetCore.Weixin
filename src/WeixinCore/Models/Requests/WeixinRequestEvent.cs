using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{

    public abstract class EventWeixinRequest : WeixinRequest, IWeixinRequest
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.@event; }
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        public virtual RequestEventType Event
        {
            get { return RequestEventType.VIEW; }
        }

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应，如果是View，则是跳转到的URL地址
        /// </summary>
        public string EventKey { get; set; }
    }
}
