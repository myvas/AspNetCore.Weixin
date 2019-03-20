using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 接收消息类型
    /// </summary>
    ///<remarks>存储时建议使用.ToString()存储字符串，不要存储数值。解析时应忽略大小写。</remarks>
    public enum ReceivedMsgType
    {
        #region 接收普通消息
        /// <summary>
        /// 100 文本消息
        /// </summary>
        text = 100,
        /// <summary>
        /// 200 图片消息
        /// </summary>
        image = 200,
        /// <summary>
        /// 300 语音消息
        /// </summary>
        voice = 300,
        /// <summary>
        /// 400 视频消息
        /// </summary>
        video = 400,
        /// <summary>
        /// 440 小视频消息
        /// </summary>
        shortvideo = 500,
        /// <summary>
        /// 500 地理位置消息
        /// </summary>
        location = 600,
        /// <summary>
        /// 6 链接消息
        /// </summary>
        link = 700,
        #endregion

        /// <summary>
        /// 接收事件推送
        /// </summary>
        @event = 200000
    }
}
