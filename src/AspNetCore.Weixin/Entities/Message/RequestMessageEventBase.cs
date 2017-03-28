using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    public interface IRequestMessageEventBase : IRequestMessageBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        WeixinEvent Event { get; }
        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        string EventKey { get; set; }
    }

    public class RequestMessageEventBase : RequestMessageBase, IRequestMessageBase
    {
        public override RequestMsgType MsgType
        {
            get { return RequestMsgType.Event; }
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        public virtual WeixinEvent Event
        {
            get { return WeixinEvent.ENTER; }
        }

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应，如果是View，则是跳转到的URL地址
        /// </summary>
        public string EventKey { get; set; }
    }
}
