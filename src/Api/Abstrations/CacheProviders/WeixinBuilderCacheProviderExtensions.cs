
using System;
using System.Linq;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection;

public static class WeixinBuilderCacheProviderExtensions
{
    public static WeixinBuilder AddWeixinMemoryCacheProvider(this WeixinBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IWeixinCacheProvider), typeof(WeixinMemoryCacheProvider)));
        return builder;
    }

    public static WeixinBuilder AddWeixinRedisCacheProvider(this WeixinBuilder builder, Action<RedisCacheOptions> setupAction = null)
    {
        if (setupAction != null)
        {
            builder.Services.Configure(setupAction);
        }
        builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IWeixinCacheProvider), typeof(WeixinRedisCacheProvider)));
        return builder;
    }

    public static WeixinBuilder AddWeixinCacheProvider<TWeixinCacheProvider>(this WeixinBuilder builder)
        where TWeixinCacheProvider : class, IWeixinCacheProvider
    {
        builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IWeixinCacheProvider), typeof(TWeixinCacheProvider)));
        return builder;
    }
}