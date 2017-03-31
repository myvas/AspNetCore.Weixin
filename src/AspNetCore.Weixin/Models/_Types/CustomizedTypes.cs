using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
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
