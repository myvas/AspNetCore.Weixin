using Myvas.AspNetCore.Weixin;
using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin
{

    /// <summary>
    /// Options for the WeixinSite middleware.
    /// </summary>
    public class WeixinSiteOptions
    {
        /// <summary>
        /// 网站令牌(Token)，用于验证开发者服务器。
        /// </summary>
        public string WebsiteToken { get; set; }

        /// <summary>
        /// 消息模式 <see cref="EncodingContants"/>
        /// </summary>
        public string EncodingType { get; set; } = WeixinSiteOptionsDefaults.DefaultEncodingType;

        /// <summary>
        /// 是否允许微信web开发工具等调试终端访问，默认为false（不允许）。
        /// </summary>
        public bool Debug { get; set; } = WeixinSiteOptionsDefaults.Debug;
    }
}
