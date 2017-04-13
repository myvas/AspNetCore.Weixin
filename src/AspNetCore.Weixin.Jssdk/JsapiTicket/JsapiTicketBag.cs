using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 微信API访问凭证数据包。全局存储及更新。
    /// </summary>
    public class JsapiTicketBag
    {
        /// <summary>
        /// 容差20分钟
        /// <para>提前20分钟将正常Token过期，以便容许系统实现出现一些时间差</para>
        /// </summary>
        private const int ToleranceInMinutes = 20;

        /// <summary>
        /// 微信全局访问凭证数据。通过调用接口获得。
        /// </summary>
        public JsapiTicketJson JsapiTicketJson = new JsapiTicketJson();

        /// <summary>
        /// 微信AccessToken
        /// </summary>
        public string AccessToken;

        /// <summary>
        /// 凭证创建/更新时间。调用接口返回时更新。
        /// </summary>
        public DateTime CreateTime = DateTime.MinValue.AddDays(1);

        /// <summary>
        /// 过期时间。通过计算得出，公式：过期时间=创建时间+凭据有效期。
        /// </summary>
        /// <remarks></remarks>
        public DateTime ExpireTime
        {
            get { return CreateTime.AddSeconds(JsapiTicketJson.expires_in); }
            set { CreateTime = value.AddSeconds(-JsapiTicketJson.expires_in); }
        }

        /// <summary>
        /// 是否已过期
        /// </summary>
        public bool IsExpired { get { return DateTime.Now > ExpireTime.AddMinutes(-ToleranceInMinutes); } }

        /// <summary>
        /// 立刻过期
        /// </summary>
        public void SetExpired() { ExpireTime = DateTime.Now.AddMinutes(-ToleranceInMinutes); }
    }
}
