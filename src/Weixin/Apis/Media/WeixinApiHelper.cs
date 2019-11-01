using Newtonsoft.Json.Linq;
using System;

namespace Myvas.AspNetCore.Weixin
{
	public static class WeixinApiHelper
    {
        #region 错误时微信会返回含有两个字段的JSON数据包
        /// <summary>
        ///  errcode
        /// </summary>
        public static int GetErrorCode(JObject payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }
            return payload.Value<int?>("errcode") ?? 0;
        }

        /// <summary>
        ///  errmsg
        /// </summary>
        public static string GetErrorMessage(JObject payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }
            return payload.Value<string>("errmsg");
        }
        #endregion
    }
}
