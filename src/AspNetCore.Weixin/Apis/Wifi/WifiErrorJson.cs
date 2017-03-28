using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace AspNetCore.Weixin
{
    /// <summary>
    /// 微信WiFi相关API返回值（JSON）
    /// </summary>
    public class WifiErrorJson : WeixinJson
    {
        /// <summary>
        /// 微信错误代码
        /// </summary>
        public int errorCode;
        /// <summary>
        /// 由于微信WiFi相关API采用了与微信基础API不一致的命名，因此，我们不得不进行一下转换。
        /// <para>WTF，麻花疼!</para>
        /// </summary>
        public int errcode { get { return errorCode; } }

        /// <summary>
        /// 微信错误描述
        /// </summary>
        public string errorMessage;
        /// <summary>
        /// 由于微信WiFi相关API采用了与微信基础API不一致的命名，因此，我们不得不进行一下转换。
        /// <para>WTF，麻花疼!</para>
        /// </summary>
        public string errmsg { get { return errorMessage; } }
    }
}
