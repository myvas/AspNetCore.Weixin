using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http.Json;
using Myvas.AspNetCore.Weixin.Site.ResponseBuilder;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Myvas.AspNetCore.Weixin.Services;
using Myvas.AspNetCore.Weixin.Services.Default;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
/// </summary>
public static class WeixinSiteServiceCollectionExtensions
{
    /// <summary>
    /// Adds weixin access token services to the specified <see cref="IServiceCollection" />. 
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IWeixinBuilder AddWeixinSite<TWeixinEventSink>(this IWeixinBuilder builder, Action<WeixinSiteOptions> setupAction)
        where TWeixinEventSink : class, IWeixinEventSink
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (setupAction != null)
        {
            builder.Services.Configure(setupAction);
        }

        builder.Services.AddSingleton<IWeixinHandlerFactory, WeixinHandlerFactory>();
        //builder.Services.TryAddScoped<IWeixinMessageEncryptor, WeixinMessageEncryptor>(); //即使不启用加密，也把此不必要的加密服务接口提供出来了。
        //builder.Services.AddWeixinMessageProtection();
        builder.Services.AddWeixinResponseBuilder();
        builder.Services.TryAddTransient<IWeixinEventSink, TWeixinEventSink>();
        builder.Services.AddSingleton<WeixinSite>();

        return builder;
    }
}

