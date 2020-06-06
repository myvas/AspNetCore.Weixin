using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Net.Http;

namespace Myvas.AspNetCore.Weixin
{
    public static class WeixinClientValidator
    {
        /// <summary>
        /// 是否来自微信客户端浏览器
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static bool IsWeixinClientRequest(this HttpContent httpContext, string weixinToken)
        {
            //List<string> keys = new List<string>(httpContext.Request.QueryString.AllKeys);
            //if (keys.Contains("signature") && keys.Contains("timestamp") && keys.Contains("nonce"))
            //{
            //    if (Signature.Check(httpContext.Request.QueryString["signature"],
            //        httpContext.Request.QueryString["timestamp"],
            //        httpContext.Request.QueryString["nonce"]
            //        , weixinToken))
            //    {
            //        return !string.IsNullOrEmpty(httpContext.Request.UserAgent) &&
            //               httpContext.Request.UserAgent.Contains("MicroMessenger");
            //    }
            //}
            return false;
        }
    }
}
