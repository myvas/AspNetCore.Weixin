
using System;
using System.Linq;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin;
using StackExchange.Redis;

namespace Microsoft.Extensions.DependencyInjection;

public static class WeixinBuilderCacheProviderExtensions
{
    public static WeixinBuilder AddMemoryCacheProvider<TWeixinCacheJson>(this WeixinBuilder builder)
        where TWeixinCacheJson : IWeixinExpirableValue, new()
    {
        builder.Services.AddMemoryCache();
        builder.Services.Where(x => x.ServiceType == typeof(IWeixinCacheProvider<TWeixinCacheJson>)).ToList()
            .ForEach(x => builder.Services.Remove(x));
        builder.Services.AddSingleton<IWeixinCacheProvider<TWeixinCacheJson>, WeixinExpirationMemoryCacheProvider<TWeixinCacheJson>>();
        return builder;
    }

    public static WeixinBuilder AddRedisCacheProvider<TWeixinCacheJson>(this WeixinBuilder builder, Action<RedisCacheOptions> setupAction = null)
        where TWeixinCacheJson : IWeixinExpirableValue, new()
    {
        if (setupAction != null)
        {
            builder.Services.Configure(setupAction);
        }

        builder.Services.Where(x => x.ServiceType == typeof(IWeixinCacheProvider<TWeixinCacheJson>)).ToList()
            .ForEach(x => builder.Services.Remove(x));
        builder.Services.AddSingleton<IWeixinCacheProvider<TWeixinCacheJson>, WeixinExpirationRedisCacheProvider<TWeixinCacheJson>>();
        return builder;
    }

    public static WeixinBuilder AddWeixinCacheProvider<TWeixinCacheJson, TWeixinCacheProvider>(this WeixinBuilder builder)
        where TWeixinCacheJson : IWeixinExpirableValue
        where TWeixinCacheProvider : class, IWeixinCacheProvider<TWeixinCacheJson>
    {
        builder.Services.Where(x => x.ServiceType == typeof(IWeixinCacheProvider<TWeixinCacheJson>)).ToList()
            .ForEach(x => builder.Services.Remove(x));
        builder.Services.AddSingleton<IWeixinCacheProvider<TWeixinCacheJson>, TWeixinCacheProvider>();
        builder.Services.AddSingleton<TWeixinCacheProvider>();
        return builder;
    }
}