using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{

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
