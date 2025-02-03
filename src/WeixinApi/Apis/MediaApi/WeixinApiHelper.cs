using System;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin
{
    public static class WeixinApiHelper
    {
        #region 错误时微信会返回含有两个字段的JSON数据包
        /// <summary>
        ///  errcode
        /// </summary>
        public static int GetErrorCode(JsonDocument payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }
            if (payload.RootElement.GetProperty("errcode").TryGetInt32(out int result))
            {
                return result;
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        ///  errmsg
        /// </summary>
        public static string GetErrorMessage(JsonDocument payload)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }
            return payload.RootElement.GetProperty("errmsg").GetString();
        }
        #endregion
    }
}
