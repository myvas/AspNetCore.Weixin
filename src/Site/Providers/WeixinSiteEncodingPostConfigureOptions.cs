using System;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Set values for <see cref="WeixinSiteEncodingOptions"/> from <see cref="WeixinSiteOptions"/>.
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public class WeixinSiteEncodingPostConfigureOptions<TOptions> : IPostConfigureOptions<TOptions>
    where TOptions : WeixinSiteEncodingOptions, new()
{
    private readonly WeixinSiteOptions _siteOptions;

    public WeixinSiteEncodingPostConfigureOptions(IOptions<WeixinSiteOptions> optionsAccessor)
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
        options.AppId = _siteOptions.AppId;
        options.WebsiteToken = _siteOptions.WebsiteToken;
        options.EncodingAESKey = _siteOptions.EncodingAESKey;
        options.StrictMode = _siteOptions.StrictMode;
    }
}
