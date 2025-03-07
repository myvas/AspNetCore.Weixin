using Microsoft.Extensions.DependencyInjection;
using Myvas.AspNetCore.Weixin;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// A builder.
/// </summary>
public class WeixinBuilder : IWeixinBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WeixinBuilder"/> class.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <exception cref="System.ArgumentNullException">services</exception>
    public WeixinBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    /// <summary>
    /// Gets the services.
    /// </summary>
    /// <value>
    /// The services.
    /// </value>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets the <see cref="Type"/> used for subscribers.
    /// </summary>
    /// <value>
    /// The <see cref="Type"/> used for subscribers.
    /// </value>
    public Type SubscriberType { get; set; }
}
