using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinAccessTokenRedisCacheProvider : IWeixinAccessTokenCacheProvider
{
    //private static readonly string CachePrefix = Guid.NewGuid().ToString("N");
    private static readonly string CachePrefix = "WX_A_TOKEN";
    private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

    private readonly IDistributedCache _cache;

    public WeixinAccessTokenRedisCacheProvider(IDistributedCache cache)
    {
        _cache = cache;
    }

    public WeixinAccessTokenJson Get(string appId)
        => Task.Run(async () => await GetAsync(appId)).Result;

    public async Task<WeixinAccessTokenJson> GetAsync(string appId, CancellationToken cancellationToken = default)
    {
        var cacheKey = GenerateCacheKey(appId);
        var accessToken = await _cache.GetAsync<WeixinAccessTokenJson>(cacheKey, cancellationToken);
        // If the expiration window is less than 2 seconds, then we need fetch new one.
        if (accessToken?.Succeeded ?? false)
        {
            if (accessToken.ExpiresIn > 2) return accessToken;
        }
        return null;
    }

    public void Remove(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);
        _cache.Remove(cacheKey);
    }

    public void Replace(string appId, WeixinAccessTokenJson json)
        => Task.Run(async () => await ReplaceAsync(appId, json));

    public async Task ReplaceAsync(string appId, WeixinAccessTokenJson json, CancellationToken cancellationToken = default)
    {
        var entryOptions = new DistributedCacheEntryOptions
        {
            // Cut off 2 seconds to avoid abnormal expiration
            SlidingExpiration = TimeSpan.FromSeconds(json.ExpiresIn - 2)
        };
        var cacheKey = GenerateCacheKey(appId);
        await _cache.SetAsync(cacheKey, json, entryOptions, cancellationToken);
    }
}
