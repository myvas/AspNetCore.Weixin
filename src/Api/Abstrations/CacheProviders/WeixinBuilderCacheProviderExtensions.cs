
using System.Linq;
using Myvas.AspNetCore.Weixin;

namespace Microsoft.Extensions.DependencyInjection;

public static class WeixinBuilderCacheProviderExtensions
{
    public static WeixinBuilder AddMemoryCacheProvider<TWeixinCacheJson>(this WeixinBuilder builder)
        where TWeixinCacheJson : IWeixinCacheJson
    {
        builder.Services.AddMemoryCache();
        builder.Services.Where(x => x.ImplementationType == typeof(IWeixinCacheProvider<TWeixinCacheJson>)).ToList()
            .ForEach(x => builder.Services.Remove(x));
        builder.Services.AddSingleton<IWeixinCacheProvider<TWeixinCacheJson>, WeixinMemoryCacheProvider<TWeixinCacheJson>>();
        return builder;
    }

    public static WeixinBuilder AddRedisCacheProvider<TWeixinCacheJson>(this WeixinBuilder builder)
        where TWeixinCacheJson : IWeixinCacheJson
    {
        builder.Services.AddMemoryCache();
        builder.Services.Where(x => x.ImplementationType == typeof(IWeixinCacheProvider<TWeixinCacheJson>)).ToList()
            .ForEach(x => builder.Services.Remove(x));
        builder.Services.AddSingleton<IWeixinCacheProvider<TWeixinCacheJson>, WeixinRedisCacheProvider<TWeixinCacheJson>>();
        return builder;
    }

    public static WeixinBuilder AddCacheProvider<TWeixinCacheJson, TWeixinCacheProvider>(this WeixinBuilder builder)
        where TWeixinCacheJson : IWeixinCacheJson
        where TWeixinCacheProvider : class, IWeixinCacheProvider<TWeixinCacheJson>
    {
        builder.Services.Where(x => x.ImplementationType == typeof(IWeixinCacheProvider<TWeixinCacheJson>)).ToList()
            .ForEach(x => builder.Services.Remove(x));
        builder.Services.AddSingleton<IWeixinCacheProvider<TWeixinCacheJson>, TWeixinCacheProvider>();
        builder.Services.AddSingleton<TWeixinCacheProvider>();
        return builder;
    }
}