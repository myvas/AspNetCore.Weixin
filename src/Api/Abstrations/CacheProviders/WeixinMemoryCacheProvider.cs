﻿using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin;

public class WeixinMemoryCacheProvider<T> : IWeixinCacheProvider<T>
    where T : IWeixinExpirableValue
{
    //private static readonly string CachePrefix = Guid.NewGuid().ToString("N");
    //private static readonly string CachePrefix = "WX_A_TOKEN";
    // typeof(T).Name.GetHashCode() has different value on different dotnet runtimes, so avoid to use it!
    //private static readonly string CachePrefix = "WX_" + typeof(T).Name.GetHashCode().ToString("X");
    private static readonly string CachePrefix = "WX_" + typeof(T).Name;
    private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

    private readonly IMemoryCache _cache;

    public WeixinMemoryCacheProvider(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T Get(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);

        if (_cache.TryGetValue(cacheKey, out string json))
        {
            var accessToken = JsonSerializer.Deserialize<T>(json);
            // If the expiration window is less than 2 seconds, then we need fetch new one.
            return (accessToken.Validate() && accessToken.ExpiresIn > 2) ? accessToken : default;
        }

        return default;
    }

    public void Remove(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);
        _cache.Remove(cacheKey);
    }

    public bool Replace(string appId, T accessToken)
    {
        var json = JsonSerializer.Serialize(accessToken);
        // Cut off 2 seconds to avoid abnormal expiration
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(accessToken.ExpiresIn - 2));
        var cacheKey = GenerateCacheKey(appId);
        _cache.Set(cacheKey, json, cacheEntryOptions);

        // To ensure the value stored
        if (_cache.TryGetValue(cacheKey, out string storedJson))
            return storedJson == json;
        return false;
    }
}
