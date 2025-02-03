using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 微信API返回值（JSON）
    /// </summary>
    public class WeixinErrorJson : WeixinJson, IWeixinError
    {
        public WeixinErrorJson()
        {

        }

        public WeixinErrorJson(int? code, string msg)
        {
            errcode = code;
            errmsg = msg;
        }

        public bool Succeeded { get { return !errcode.HasValue || errcode == WeixinErrorCodes.OK; } }

        /// <summary>
        /// 微信错误代码
        /// </summary>
        public int? errcode { get; set; }

        /// <summary>
        /// 微信错误描述
        /// </summary>
        public string errmsg { get; set; }
    }
}
