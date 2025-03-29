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
