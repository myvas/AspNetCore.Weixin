using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin;

public class WeixinAccessTokenMemoryCacheProvider : IWeixinAccessTokenCacheProvider
{
    //private static readonly string CachePrefix = Guid.NewGuid().ToString("N");
    private static readonly string CachePrefix = "WX_A_TOKEN";
    private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

    private readonly IMemoryCache _cache;

    public WeixinAccessTokenMemoryCacheProvider(IMemoryCache cache)
    {
        _cache = cache;
    }

    public WeixinAccessTokenJson Get(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);
        
        if (_cache.TryGetValue(cacheKey, out string json))
        {
            var accessToken = JsonSerializer.Deserialize<WeixinAccessTokenJson>(json);
            // If the expiration window is less than 2 seconds, then we need fetch new one.
            return (accessToken.Succeeded && accessToken.ExpiresIn > 2) ? accessToken : null;
        }
        
        return null;
    }

    public void Remove(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);
        _cache.Remove(cacheKey);
    }

    public void Replace(string appId, WeixinAccessTokenJson accessToken)
    {
        var json = JsonSerializer.Serialize(accessToken);
        // Cut off 2 seconds to avoid abnormal expiration
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(accessToken.ExpiresIn - 2));
        var cacheKey = GenerateCacheKey(appId);
        _cache.Set(cacheKey, json, cacheEntryOptions);
    }
}
