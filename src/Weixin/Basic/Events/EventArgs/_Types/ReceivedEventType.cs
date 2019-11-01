using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 当MsgType字段值为<see cref="ReceivedMsgType.@event"/>时，Event字段值即为事件类型(<see cref="ReceivedEventType"/>)
    /// </summary>
    /// <remarks>存储时建议使用.ToString()，而非数值。解析时应忽略大小写。</remarks>
    public enum ReceivedEventType
    {
        /// <summary>
        /// 1.1 订阅
        /// </summary>
        subscribe = 100,

		/// <summary>
		/// 1.2 取消订阅
		/// </summary>
		unsubscribe = 200,

        /// <summary>
        /// 300 二维码扫描
        /// </summary>
        SCAN = 300,

        /// <summary>
        /// 400 上报地理位置事件，注意区别于“地理位置上行消息”
        /// </summary>
        LOCATION = 400,

        /// <summary>
        /// 500 自定义菜单事件：拉取消息
        /// </summary>
        CLICK = 500,

        /// <summary>
        /// 600 自定义菜单事件：跳转链接
        /// </summary>
        VIEW = 600
    }
}
