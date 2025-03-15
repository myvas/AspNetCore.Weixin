using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Helper functions for configuring Weixin services.
/// </summary>
public class WeixinSiteBuilder
{
    public WeixinBuilder WeixinBuilder { get; private set; }

    public WeixinSiteBuilder(WeixinBuilder weixinBuilder)
    {
        WeixinBuilder = weixinBuilder;
    }

    public IServiceCollection Services { get => WeixinBuilder.Services; }

    private IList<IWeixinSite> _handlers = new List<IWeixinSite>();
    public IReadOnlyList<IWeixinSite> WeixinHandlers { get { return (IReadOnlyList<IWeixinSite>)_handlers; } }
    public IWeixinSite AddHandler(IWeixinSite handler)
    {
        if (!_handlers.Contains(handler))
        {
            _handlers.Add(handler);
        }
        return handler;
    }

    //public OptionsBuilder<WeixinOptions> ApiOptions { get=>WeixinBuilder.Options; }
    public OptionsBuilder<WeixinSiteOptions> SiteOptions { get; set; }
    public OptionsBuilder<WeixinSiteEncodingOptions> EncodingOptions { get; set; }

    /// <summary>
    /// Gets the <see cref="Type"/> used for Weixin subscriber.
    /// </summary>
    /// <value>
    /// The <see cref="Type"/> used for Weixin subscriber.
    /// </value>
    public Type ExternalUserType { get; private set; }

    /// <summary>
    /// Gets the <see cref="Type"/> used for Weixin subscriber.
    /// </summary>
    /// <value>
    /// The <see cref="Type"/> used for Weixin subscriber.
    /// </value>
    public Type ExternalUserIdType { get; private set; }
}
