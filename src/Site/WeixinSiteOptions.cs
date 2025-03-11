using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Options for the WeixinSite middleware.
/// </summary>
/// <seealso cref="WeixinOptions"/>
public class WeixinSiteOptions
{
    /// <summary>
    /// 网站令牌(Token)，用于验证开发者服务器。
    /// </summary>
    public string WebsiteToken { get; set; }

    /// <summary>
    /// 服务器地址路径，默认为: <see cref="WeixinSiteOptionsDefaults.Path"/>
    /// </summary>
    public PathString Path { get; set; } = WeixinSiteOptionsDefaults.Path;

    public WeixinSiteEncodingOptions Encoding { get; set; } = new WeixinSiteEncodingOptions();

    /// <summary>
    /// 是否允许微信web开发工具等调试终端访问，默认为: <see cref="WeixinSiteOptionsDefaults.Debug"/> 。
    /// </summary>
    public bool Debug { get; set; } = WeixinSiteOptionsDefaults.Debug;

    /// <summary>
    /// 接收微信消息或事件
    /// </summary>
    public WeixinEvents Events { get; set; }

    /// <summary>
    /// 最大请求内容长度，默认为: <see cref="WeixinSiteOptionsDefaults.MaxRequestContentLength">。
    /// </summary>internal
    public int MaxRequestContentLength { get; set; }

    public WeixinSiteOptions()
    {
        Events = new WeixinEvents();
    }
}
