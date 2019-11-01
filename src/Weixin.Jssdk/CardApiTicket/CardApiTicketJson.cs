
using AspNetCore.Weixin;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 微信全局访问票据数据。调用微信API接口时必须。</summary>
    /// <example>
    /// 正常情况下，微信会返回下述JSON数据包给公众号：
    /// <code>
    /// {"ticket":"bxLdikRXVbTPdHSM05e5u5sUoXNKd8-41ZO3MhKoyN5OfkWITDGgnr2fwJ0m9E8NYzWKVZvdVtaUgWvsdshFKA","expires_in":7200}</code>
    /// </example>
    public class CardApiTicketJson : WeixinErrorJson
    {
        /// <summary>
        /// weixin jsapi ticket
        /// </summary>
        /// <example>bxLdikRXVbTPdHSM05e5u5sUoXNKd8-41ZO3MhKoyN5OfkWITDGgnr2fwJ0m9E8NYzWKVZvdVtaUgWvsdshFKA</example>
        public string ticket;

        /// <summary>
        /// 凭证有效时间。单位：秒。
        /// <para>例如：7200</para>
        /// </summary>
        /// <example></example>
        public int expires_in = 7200;
    }
}
