using Myvas.AspNetCore.Weixin;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace Myvas.AspNetCore.Weixin
{

    /// <summary>
    /// Options for the WeixinSite middleware.
    /// </summary>
    public class WeixinSiteOptions
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }

        /// <summary>
        /// 网站令牌(Token)，用于验证开发者服务器。
        /// </summary>
        public string WebsiteToken { get; set; }

        /// <summary>
        /// 服务器地址路径，默认为: /wx
        /// </summary>
        public PathString Path { get; set; } = WeixinSiteOptionsDefaults.Path;

        public WeixinSiteEncodingOptions Encoding { get; set; } = new WeixinSiteEncodingOptions();

        /// <summary>
        /// 是否允许微信web开发工具等调试终端访问，默认为: false（不允许）。
        /// </summary>
        public bool Debug { get; set; } = WeixinSiteOptionsDefaults.Debug;

        /// <summary>
        /// 接收微信消息或事件
        /// </summary>
        public WeixinEvents Events { get; set; }

        /// <summary>
        /// Used to communicate with the remote tencent server.
        /// </summary>
        public HttpClient Backchannel { get; set; }

        public WeixinSiteOptions()
        {
            Events = new WeixinEvents();
        }
    }
}
