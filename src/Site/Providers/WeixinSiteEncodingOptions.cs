namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Options for the WeixinSite encoding provider <see cref="WeixinMessageEncryptor"/>.
/// </summary>
/// <remarks>This options is created in <see cref="WeixinSitePostConfigureOptions"/>.</remarks>
public class WeixinSiteEncodingOptions
{
    /// <summary>
    /// The AppId comes from <see cref="WeixinOptions"/>.
    /// </summary>
    public string AppId { get; internal set; }

    /// <summary>
    /// The WeixinSiteToken for WeixinSite verification comes from <see cref="WeixinSiteOptions"/>.
    /// </summary>
    public string WebsiteToken { get; internal set; }

    /// <summary>
    /// 消息加解密密钥
    /// </summary>
    public string EncodingAESKey { get; set; }

    /// <summary>
    /// 采用严格模式，即：所有消息必须加密，不允许明文消息
    /// <para>default is <see cref="WeixinSiteOptionsDefaults.StrictMode"/>  which is false</para>
    /// </summary>
    public bool StrictMode { get; set; } = WeixinSiteOptionsDefaults.StrictMode;
}
