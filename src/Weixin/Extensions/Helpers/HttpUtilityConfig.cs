﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin.Helpers
{
    /// <summary>
    /// 全局设置
    /// </summary>
    internal static class HttpUtilityConfig
    {
        /// <summary>
        /// 请求超时设置（以毫秒为单位），默认为10秒
        /// </summary>
        public const int TIME_OUT = 10000;
    }
}
