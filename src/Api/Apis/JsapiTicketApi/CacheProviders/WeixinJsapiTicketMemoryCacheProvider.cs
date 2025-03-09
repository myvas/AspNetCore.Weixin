using Microsoft.Extensions.Caching.Memory;
using System;

namespace Myvas.AspNetCore.Weixin;

public class WeixinJsapiTicketMemoryCacheProvider : IWeixinJsapiTicketCacheProvider
{
    //private static readonly string CachePrefix = Guid.NewGuid().ToString("N");
    private static readonly string CachePrefix = "WX_J_TICKET";
    private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

    private readonly IMemoryCache _cache;

    public WeixinJsapiTicketMemoryCacheProvider(IMemoryCache cache)
    {
        _cache = cache;
    }

    public WeixinJsapiTicketJson Get(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);
        if (_cache.TryGetValue(cacheKey, out WeixinJsapiTicketJson accessToken))
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

    public void Replace(string appId, WeixinJsapiTicketJson json)
    {
        var accessToken = json.Ticket;
        var absoluteExpirationRelativeToNow = TimeSpan.FromSeconds(json.ExpiresIn);
        var cacheKey = GenerateCacheKey(appId);
        _cache.Set(cacheKey, accessToken, absoluteExpirationRelativeToNow);
    }
}
