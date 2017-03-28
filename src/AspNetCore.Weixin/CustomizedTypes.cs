using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 接收消息类型
    /// </summary>
    public enum RequestMsgType
    {
        #region 接收普通消息
        /// <summary>
        /// 1 文本消息
        /// </summary>
        Text,
        /// <summary>
        /// 5 地理位置消息
        /// </summary>
        Location,
        /// <summary>
        /// 2 图片消息
        /// </summary>
        Image,
        /// <summary>
        /// 3 语音消息
        /// </summary>
        Voice,
        /// <summary>
        /// 4 视频消息
        /// </summary>
        Video,
        /// <summary>
        /// 6 链接消息
        /// </summary>
        Link,
        #endregion

        /// <summary>
        /// 接收事件推送
        /// </summary>
        Event,
    }

    /// <summary>
    /// 回复/发送消息类型
    /// </summary>
    public enum ResponseMsgType
    {
        /// <summary>
        /// 1 回复/发送文本消息
        /// </summary>
        Text,
        /// <summary>
        /// 6 回复/发送图文消息
        /// </summary>
        News,
        /// <summary>
        /// 5 回复/发送音乐消息
        /// </summary>
        Music,
        /// <summary>
        /// 2 回复/发送图片消息
        /// </summary>
        Image,
        /// <summary>
        /// 3 回复/发送语音消息
        /// </summary>
        Voice,
        /// <summary>
        /// 4 回复/发送视频消息
        /// </summary>
        Video
    }

    /// <summary>
    /// 当RequestMsgType类型为Event时，Event属性的类型
    /// </summary>
    public enum WeixinEvent
    {
        /// <summary>
        /// 进入会话（似乎已从官方API中移除）
        /// </summary>
        ENTER,

        /// <summary>
        /// 3 地理位置（似乎已从官方API中移除）
        /// </summary>
        LOCATION,

        /// <summary>
        /// 1.1 订阅
        /// </summary>
        subscribe,

        /// <summary>
        /// 1.2 取消订阅
        /// </summary>
        unsubscribe,

        /// <summary>
        /// 4 自定义菜单事件：拉取消息
        /// </summary>
        CLICK,

        /// <summary>
        /// 2 二维码扫描
        /// </summary>
        scan,

        /// <summary>
        /// 5 自定义菜单事件：跳转链接
        /// </summary>
        VIEW
    }

    /// <summary>
    /// 菜单按钮类型
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// 自定义菜单事件：拉取消息
        /// </summary>
        click,
        /// <summary>
        /// 自定义菜单事件：跳转链接
        /// </summary>
        view
    }

    ///// <summary>
    ///// 群发消息返回状态
    ///// </summary>
    //public enum GroupMessageStatus
    //{
    //    //高级群发消息的状态
    //    涉嫌广告 = 10001,
    //    涉嫌政治 = 20001,
    //    涉嫌社会 = 20004,
    //    涉嫌色情 = 20002,
    //    涉嫌违法犯罪 = 20006,
    //    涉嫌欺诈 = 20008,
    //    涉嫌版权 = 20013,
    //    涉嫌互推 = 22000,
    //    涉嫌其他 = 21000
    //}


}
