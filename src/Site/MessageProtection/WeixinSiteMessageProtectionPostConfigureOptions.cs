using System;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Set values for <see cref="WeixinSiteMessageProtectionOptions"/> from <see cref="WeixinSiteOptions"/>.
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public class WeixinSiteMessageProtectionPostConfigureOptions<TOptions> : IPostConfigureOptions<TOptions>
    where TOptions : WeixinSiteMessageProtectionOptions, new()
{
    private readonly WeixinSiteOptions _siteOptions;

    public WeixinSiteMessageProtectionPostConfigureOptions(IOptions<WeixinSiteOptions> optionsAccessor)
    {
        _siteOptions = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
    }

    /// <summary>
    /// Validate the TOptions instance, and set default values if necessary.
    /// </summary>
    /// <param name="name">If the options instance is not named, this parameter will be <see cref="Options.DefaultName"/>, which is an empty string("").</param>
    /// <param name="options"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void PostConfigure(string name, TOptions options)
    {
        if ((options.EncodingAESKey?.Length ?? 0) != 43)
        {
            throw WeixinMessageEncryptorError.IllegalAesKey();
        }

        options.AppId = _siteOptions.AppId;
        options.WebsiteToken = _siteOptions.WebsiteToken;
    }
}
