using System;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Validate and set default values for <see cref="TOptions"/> inherited from <see cref="WeixinSiteOptions"/>.
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public class WeixinSitePostConfigureOptions<TOptions> : IPostConfigureOptions<TOptions>
    where TOptions : WeixinSiteOptions, new()
{
    private readonly WeixinOptions _wxOptions;

    public WeixinSitePostConfigureOptions(IOptions<WeixinOptions> optionsAccessor)
    {
        _wxOptions = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
    }

    /// <summary>
    /// Validate the TOptions instance, and set default values if necessary.
    /// </summary>
    /// <param name="name">If the options instance is not named, this parameter will be <see cref="Options.DefaultName"/>, which is an empty string("").</param>
    /// <param name="options"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void PostConfigure(string name, TOptions options)
    {
        options.AppId = _wxOptions.AppId;

        if (options == null) throw new ArgumentNullException(nameof(options));

        if (string.IsNullOrEmpty(options.Path.Value))
            options.Path = WeixinSiteOptionsDefaults.Path;

        if (options.MaxRequestContentLength <= 0)
            options.MaxRequestContentLength = WeixinSiteOptionsDefaults.MaxRequestContentLength;
    }
}
