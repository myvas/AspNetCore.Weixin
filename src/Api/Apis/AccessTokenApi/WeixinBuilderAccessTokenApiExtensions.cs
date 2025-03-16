using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinBuilderAccessTokenApiExtensions{

    public static WeixinBuilder AddWeixinAccessTokenApi(this WeixinBuilder builder)
    {
        builder.Services.AddSingleton<WeixinAccessTokenDirectApi>();
        builder.Services.AddTransient<IWeixinAccessTokenApi, WeixinAccessTokenApi>();
        return builder;
    }
    public static WeixinBuilder AddAccessTokenMemoryCacheProvider(this WeixinBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.AddMemoryCacheProvider<WeixinAccessTokenJson>();
        return builder;
    }

    public static WeixinBuilder AddAccessTokenRedisCacheProvider(this WeixinBuilder builder, Action<RedisCacheOptions> setupAction = null)
    {
        //builder.Services.Add(ServiceDescriptor.Singleton<IDistributedCache, Microsoft.Extensions.Caching.StackExchangeRedis.RedisCache>());
        builder.Services.AddStackExchangeRedisCache(setupAction); // IDistributedCache
        builder.AddRedisCacheProvider<WeixinAccessTokenJson>();
        return builder;
    }

    public static WeixinBuilder AddAccessTokenCacheProvider<TWeixinCacheProvider>(this WeixinBuilder builder)
        where TWeixinCacheProvider : class, IWeixinCacheProvider<WeixinAccessTokenJson>
    {
        builder.AddWeixinCacheProvider<WeixinAccessTokenJson, TWeixinCacheProvider>();
        return builder;
    }
}
