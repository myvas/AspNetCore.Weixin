using Microsoft.Extensions.Caching.StackExchangeRedis;
using Myvas.AspNetCore.Weixin;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
/// </summary>
public static class WeixinBuildCardTicketApiExtensions
{
    /// <summary>
    /// Adds <see cref="IWeixinCardTicketApi" to the container <see cref="WeixinBuilder.Services" />. 
    /// </summary>
    /// <param name="builder">The <see cref="WeixinBuilder"/> which contains an <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="WeixinBuilder"/> so that additional calls can be chained.</returns>
    public static WeixinBuilder AddWeixinCardTicketApi(this WeixinBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.AddSingleton<WeixinCardTicketDirectApi>();
        builder.Services.AddTransient<IWeixinCardTicketApi, WeixinCardTicketApi>();
        return builder;
    }

    public static WeixinBuilder AddCardTicketMemoryCacheProvider(this WeixinBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.AddMemoryCacheProvider<WeixinCardTicketJson>();
        return builder;
    }

    public static WeixinBuilder AddCardTicketRedisCacheProvider(this WeixinBuilder builder, Action<RedisCacheOptions> setupAction = null)
    {
        builder.AddRedisCacheProvider<WeixinCardTicketJson>(setupAction);
        return builder;
    }

    public static WeixinBuilder AddCardTicketCacheProvider<TWeixinCacheProvider>(this WeixinBuilder builder)
        where TWeixinCacheProvider : class, IWeixinCacheProvider<WeixinCardTicketJson>
    {
        builder.AddWeixinCacheProvider<WeixinCardTicketJson, TWeixinCacheProvider>();
        return builder;
    }
}
