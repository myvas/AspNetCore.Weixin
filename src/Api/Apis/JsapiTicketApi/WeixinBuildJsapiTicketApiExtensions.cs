using Microsoft.Extensions.Caching.StackExchangeRedis;
using Myvas.AspNetCore.Weixin;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
/// </summary>
public static class WeixinBuildJsapiTicketApiExtensions
{
    /// <summary>
    /// Adds <see cref="IWeixinJsapiTicketApi" to the container <see cref="WeixinBuilder.Services" />. 
    /// </summary>
    /// <param name="builder">The <see cref="WeixinBuilder"/> which contains an <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="WeixinBuilder"/> so that additional calls can be chained.</returns>
    public static WeixinBuilder AddWeixinJsapiTicketApi(this WeixinBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.AddSingleton<WeixinJsapiTicketDirectApi>();
        builder.Services.AddTransient<IWeixinJsapiTicketApi, WeixinJsapiTicketApi>();
        return builder;
    }

    public static WeixinBuilder AddJsapiTicketMemoryCacheProvider(this WeixinBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.AddMemoryCacheProvider<WeixinJsapiTicketJson>();
        return builder;
    }

    public static WeixinBuilder AddJsapiTicketRedisCacheProvider(this WeixinBuilder builder, Action<RedisCacheOptions> setupAction = null)
    {
        builder.AddRedisCacheProvider<WeixinJsapiTicketJson>(setupAction);
        return builder;
    }

    public static WeixinBuilder AddJsapiTicketCacheProvider<TWeixinCacheProvider>(this WeixinBuilder builder)
        where TWeixinCacheProvider : class, IWeixinCacheProvider<WeixinJsapiTicketJson>
    {
        builder.AddWeixinCacheProvider<WeixinJsapiTicketJson, TWeixinCacheProvider>();
        return builder;
    }
}
