using Microsoft.Extensions.Caching.Memory;
using System;

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
        if (_cache.TryGetValue(cacheKey, out WeixinAccessTokenJson accessToken))
        {
            return accessToken;
        }
        else
        {
            return null;
        }
    }

    public void Remove(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);
        _cache.Remove(cacheKey);
    }

    public void Replace(string appId, string accessToken, TimeSpan absoluteExpirationRelativeToNow)
    {
        var cacheKey = GenerateCacheKey(appId);
        _cache.Set(cacheKey, accessToken, absoluteExpirationRelativeToNow);
    }

    public void Replace(string appId, WeixinAccessTokenJson json)
    {
        var accessToken = json.access_token;
        var absoluteExpirationRelativeToNow = TimeSpan.FromSeconds(json.expires_in);
        var cacheKey = GenerateCacheKey(appId);
        _cache.Set(cacheKey, accessToken, absoluteExpirationRelativeToNow);
    }
}
