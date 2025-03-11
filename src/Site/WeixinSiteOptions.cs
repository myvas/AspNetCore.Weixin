using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Options for the WeixinSite middleware.
/// </summary>
/// <seealso cref="WeixinOptions"/>
public class WeixinSiteOptions
{
    /// <summary>
    /// The AppId will be configured in <see cref="WeixinSitePostConfigureOptions"/> to the value comes from <see cref="WeixinOptions"/>.
    /// </summary>
    public string AppId { get; internal set; }

    /// <summary>
    /// 网站令牌(Token)，用于验证开发者服务器。
    /// </summary>
    public string WebsiteToken { get; set; }

    /// <summary>
    /// 服务器地址路径，默认为: <see cref="WeixinSiteOptionsDefaults.Path"/>
    /// </summary>
    public PathString Path { get; set; } = WeixinSiteOptionsDefaults.Path;

    /// <summary>
    /// 采用严格模式，即：所有消息必须加密，不允许明文消息. Default is <see cref="WeixinSiteOptionsDefaults.StrictMode"/>, which is false.
    /// </summary>
    public bool StrictMode { get; set; } = false;

    /// <summary>
    /// 消息加解密密钥
    /// </summary>
    public string EncodingAESKey { get; set; }

    /// <summary>
    /// Determines whether enable debug mode to skip the signature verification and User-Agent checking. Default is <see cref="WeixinSiteOptionsDefaults.Debug"/>, which is false.
    /// </summary>
    public bool Debug { get; set; } = WeixinSiteOptionsDefaults.Debug;

    /// <summary>
    /// 接收微信消息或事件
    /// </summary>
    public WeixinEvents Events { get; set; }

    /// <summary>
    /// The max length of Weixin request content, default is <see cref="WeixinSiteOptionsDefaults.MaxRequestContentLength">, which is 32MB.
    /// </summary>internal
    public long MaxRequestContentLength { get; set; } = WeixinSiteOptionsDefaults.MaxRequestContentLength;

    /// <summary>
    /// The max length of Weixin response buffer size, default is <see cref="WeixinSiteOptionsDefaults.MaxResponseContentBufferSize"/>, which is 32MB.
    /// </summary>
    public long MaxResponseContentBufferSize { get; set; } = WeixinSiteOptionsDefaults.MaxResponseContentBufferSize;

    public WeixinSiteOptions()
    {
        Events = new WeixinEvents();
    }
}
