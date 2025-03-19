using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Use IDatabase direct access.
/// </summary>
/// <typeparam name="T"></typeparam>
public class WeixinExpirationRedisCacheProvider<T> : IWeixinCacheProvider<T>
    where T : IWeixinExpirableValue, new()
{
    //private static readonly string CachePrefix = Guid.NewGuid().ToString("N");
    //private static readonly string CachePrefix = "WX_A_TOKEN";
    // typeof(T).Name.GetHashCode() has different value on different dotnet runtimes, so avoid to use it!
    //private static readonly string CachePrefix = "WX_" + typeof(T).Name.GetHashCode().ToString("X");
    private static readonly string CachePrefix = "WX_" + typeof(T).Name;
    private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

    private readonly RedisCacheOptions _options;
    private static IDatabase _cache;
    private static readonly object _lock = new object(); // Mutex lock object
    private IDatabase GetDatabase()
    {
        if (_cache == null)
        {
            lock (_lock) // Ensure only one thread can enter this block at a time
            {
                if (_cache == null) // Double-check to prevent race conditions
                {
                    _cache = ConnectionMultiplexer.Connect(_options.Configuration)?.GetDatabase();
                }
            }
        }
        return _cache;
    }

    public WeixinExpirationRedisCacheProvider(IOptions<RedisCacheOptions> optionsAccessor)
    {
        _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        _cache = GetDatabase() ?? throw new ArgumentNullException(nameof(optionsAccessor));
    }

    public T Get(string appId)
        => Task.Run(async () => await GetAsync(appId)).Result;

    public async Task<T> GetAsync(string appId, CancellationToken cancellationToken = default)
    {
        var cacheKey = GenerateCacheKey(appId);
        var expirableValue = new T();
        var value = await GetDatabase()?.StringGetAsync(cacheKey);
        expirableValue.Value = value;
        if (!string.IsNullOrEmpty(value))
        {
            expirableValue.ExpiresIn = ((int?)(await GetDatabase()?.KeyTimeToLiveAsync(cacheKey))?.TotalSeconds) ?? 0;
            if (expirableValue.Validate()) return expirableValue;
        }
        return expirableValue;
    }

    public void Remove(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);
        GetDatabase()?.KeyDeleteAsync(cacheKey);
    }

    public bool Replace(string appId, T expirableValue)
        => Task.Run(async () => await ReplaceAsync(appId, expirableValue)).Result;

    public async Task<bool> ReplaceAsync(string appId, T expirableValue, CancellationToken cancellationToken = default)
    {
        var cacheKey = GenerateCacheKey(appId);
        await GetDatabase()?.StringSetAsync(cacheKey, expirableValue.Value, TimeSpan.FromSeconds(expirableValue.ExpiresIn - 1));

        // To ensure value stored
        var storedValue = await GetDatabase()?.StringGetAsync(cacheKey);
        return storedValue == expirableValue.Value;
    }
}
