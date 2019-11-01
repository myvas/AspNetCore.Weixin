using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 需要推送给腾讯服务器的AP心跳数据
    /// </summary>
    public class ApOnlineData
    {
        /// <summary>
        /// AP设备编号
        /// </summary>
        public string deviceNo;
        /// <summary>
        /// AP状态。0不在线，1在线
        /// </summary>
        public int apStatus;
        /// <summary>
        /// AP连接用户数
        /// </summary>
        public int apOnlines;
    }
}
