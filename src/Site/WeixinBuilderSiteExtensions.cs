using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Extension methods for setting up <see cref="WeixinBuilder"/>.
/// </summary>
public static class WeixinBuilderExtensions
{
    /// <summary>
    /// Create a <see cref="WeixinSiteBuilder"/> with <see cref="WeixinSiteOptions">. 
    /// </summary>
    /// <param name="builder">The <see cref="WeixinBuilder"/> which contains an <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinSiteOptions"/>.</param>
    /// <returns>The <see cref="WeixinSiteBuilder"/> which contains a <see cref="WeixinBuilder"/> to access <see cref="WeixinBuilder.Services"/>.</returns>
    /// <seealso cref="WeixinBuilder"/>
    /// <seealso cref="WeixinSiteOptions"/>
    public static WeixinSiteBuilder AddWeixinSite(this WeixinBuilder builder, Action<WeixinSiteOptions> setupAction)
    {
        if (builder?.Services == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (setupAction != null)
        {
            builder.Services.Configure(setupAction);
        }

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<WeixinSiteOptions>, WeixinSitePostConfigureOptions<WeixinSiteOptions>>());

        builder.Services.AddTransient<IWeixinSite, WeixinSite>();
        builder.Services.AddTransient<IWeixinEventSink,WeixinDebugEventSink>();

        return new WeixinSiteBuilder(builder);
    }
}
