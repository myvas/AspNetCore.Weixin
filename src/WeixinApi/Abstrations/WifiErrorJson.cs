namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 微信WiFi相关API返回值（JSON）
    /// </summary>
    public class WifiErrorJson : WeixinJson, IWeixinError
    {
        /// <summary>
        /// 微信错误代码
        /// </summary>
        public int? errorCode { get; set; }

        /// <summary>
        /// 微信错误描述
        /// </summary>
        public string errorMessage { get; set; }

        #region 不同定义之间的转换
        /// <summary>
        /// 由于微信WiFi相关API采用了与微信基础API不一致的命名，因此，我们不得不进行一下转换。
        /// <para>WTF，麻花疼!</para>
        /// </summary>
        public int? errcode { get { return errorCode; } }

        /// <summary>
        /// 由于微信WiFi相关API采用了与微信基础API不一致的命名，因此，我们不得不进行一下转换。
        /// <para>WTF，麻花疼!</para>
        /// </summary>
        public string errmsg { get { return errorMessage; } }

        public bool Succeeded { get { return !errcode.HasValue || errcode == WeixinErrorCodes.OK; } }
        #endregion
    }
}
