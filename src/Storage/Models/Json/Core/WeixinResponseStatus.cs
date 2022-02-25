using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 微信响应状态
    /// </summary>
    public static class WeixinResponseStatus
    {
        /// <summary>
        /// 正常, Code: 0
        /// </summary>
        public const int OK = 0;
        /// <summary>
        /// 微信错误，Code: 4xxxx-5xxxx
        /// 特别地，系统繁忙, Code: -1
        /// </summary>
        public const int WeixinError = -10000;
        /// <summary>
        /// 自定义错误, Code: -4xxxx
        /// </summary>
        public const int CustomError = -40000;
    }
}
