using Myvas.AspNetCore.Weixin;
using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinSiteEncoderOptions
    {
        /// <summary>
        /// 消息模式 <see cref="EncryptionModes"/>
        /// </summary>
        public string EncryptionMode { get; set; } = WeixinSiteOptionsDefaults.DefaultEncryptionMode;

        /// <summary>
        /// 消息加解密密钥
        /// </summary>
        public string EncodingAESKey { get; set; }
    }

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
        /// 服务器地址路径，默认为: /wx
        /// </summary>
        public PathString Path { get; set; } = WeixinSiteOptionsDefaults.Path;

        public WeixinSiteEncoderOptions SiteEncoder { get; set; } = new WeixinSiteEncoderOptions();

        /// <summary>
        /// 是否允许微信web开发工具等调试终端访问，默认为: false（不允许）。
        /// </summary>
        public bool Debug { get; set; } = WeixinSiteOptionsDefaults.Debug;

        /// <summary>
        /// 接收微信消息或事件
        /// </summary>
        public WeixinEvents Events { get; set; }

        public WeixinSiteOptions()
        {
            Events = new WeixinEvents();
        }
    }
}
