using Myvas.AspNetCore.Weixin;
using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin
{

    /// <summary>
    /// Options for the WeixinSite middleware.
    /// </summary>
    public class SiteOptions
    {
        /// <summary>
        /// 网站令牌(Token)，用于验证开发者服务器。
        /// </summary>
        public string WebsiteToken { get; set; }

        /// <summary>
        /// 服务器地址路径，默认为: /wx
        /// </summary>
        public string Path { get; set; } = WeixinSiteOptionsDefaults.Path;

        /// <summary>
        /// 消息模式 <see cref="EncodingContants"/>
        /// </summary>
        public string EncodingType { get; set; } = WeixinSiteOptionsDefaults.DefaultEncodingType;

        /// <summary>
        /// 是否允许微信web开发工具等调试终端访问，默认为: false（不允许）。
        /// </summary>
        public bool Debug { get; set; } = WeixinSiteOptionsDefaults.Debug;

        /// <summary>
        /// 接收微信消息或事件
        /// </summary>
        public WeixinEvents Events { get; set; }

        public SiteOptions()
        {
            Events = new WeixinEvents();
        }
    }
}
