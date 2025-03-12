using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinRedisCacheProvider<T> : IWeixinCacheProvider<T>
    where T : IWeixinCacheJson
{
    //private static readonly string CachePrefix = Guid.NewGuid().ToString("N");
    //private static readonly string CachePrefix = "WX_A_TOKEN";
    // typeof(T).Name.GetHashCode() has different value on different dotnet runtimes, so avoid to use it!
    //private static readonly string CachePrefix = "WX_" + typeof(T).Name.GetHashCode().ToString("X");
    private static readonly string CachePrefix = "WX_" + typeof(T).Name;
    private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

    private readonly IDistributedCache _cache;

    public WeixinRedisCacheProvider(IDistributedCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public T Get(string appId)
        => Task.Run(async () => await GetAsync(appId)).Result;

    public async Task<T> GetAsync(string appId, CancellationToken cancellationToken = default)
    {
        var cacheKey = GenerateCacheKey(appId);
        var accessToken = await _cache.GetAsync<T>(cacheKey, cancellationToken);
        // If the expiration window is less than 2 seconds, then we need fetch new one.
        if (accessToken?.Succeeded ?? false)
        {
            if (accessToken.ExpiresIn > 2) return accessToken;
        }
        return default;
    }

    public void Remove(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);
        _cache.Remove(cacheKey);
    }

    public void Replace(string appId, T json)
        => Task.Run(async () => await ReplaceAsync(appId, json));

    public async Task ReplaceAsync(string appId, T json, CancellationToken cancellationToken = default)
    {
        var entryOptions = new DistributedCacheEntryOptions
        {
            // Cut off 2 seconds to avoid abnormal expiration
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(json.ExpiresIn - 2)
        };
        var cacheKey = GenerateCacheKey(appId);
        await _cache.SetAsync(cacheKey, json, entryOptions, cancellationToken);
    }
}
