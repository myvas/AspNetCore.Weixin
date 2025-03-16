using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinExpirationRedisCacheProvider<T> : IWeixinCacheProvider<T>
    where T : IWeixinExpirableValue, new()
{
    //private static readonly string CachePrefix = Guid.NewGuid().ToString("N");
    //private static readonly string CachePrefix = "WX_A_TOKEN";
    // typeof(T).Name.GetHashCode() has different value on different dotnet runtimes, so avoid to use it!
    //private static readonly string CachePrefix = "WX_" + typeof(T).Name.GetHashCode().ToString("X");
    private static readonly string CachePrefix = "WX_" + typeof(T).Name;
    private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

    private readonly IDistributedCache _cache;

    public WeixinExpirationRedisCacheProvider(IDistributedCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public T Get(string appId)
        => Task.Run(async () => await GetAsync(appId)).Result;

    public async Task<T> GetAsync(string appId, CancellationToken cancellationToken = default)
    {
        var cacheKey = GenerateCacheKey(appId);
        var expirableValue = new T();
        var value = await _cache.GetStringAsync(cacheKey, cancellationToken);
        expirableValue.Value = value;
        if (!string.IsNullOrEmpty(value))
        {
            expirableValue.ExpiresIn = GetAbsoluteExpiration(cacheKey);
            if (expirableValue.Validate()) return expirableValue;
        }
        return expirableValue;
    }

    public void Remove(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);
        _cache.Remove(cacheKey);
    }

    public void Replace(string appId, T expirableValue)
        => Task.Run(async () => await ReplaceAsync(appId, expirableValue));

    public async Task ReplaceAsync(string appId, T expirableValue, CancellationToken cancellationToken = default)
    {
        var cacheKey = GenerateCacheKey(appId);
        var entryOptions = new DistributedCacheEntryOptions
        {
            // Cut off 2 seconds to avoid abnormal expiration
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirableValue.ExpiresIn - 1)
        };
        await _cache.SetStringAsync(cacheKey, expirableValue.Value, entryOptions, cancellationToken);
    }

    /// <summary>
    /// Get AbsoluteExpiration property value
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private int GetAbsoluteExpiration(string cacheKey)
    {
        var memoryCacheType = _cache.GetType();
        var databaseField = memoryCacheType.GetField("_cache", BindingFlags.NonPublic | BindingFlags.Instance);
        if (databaseField == null)
        {
            throw new InvalidOperationException("Unable to access the internal _cache field of RedisCache.");
        }

        var redisDatabase = databaseField.GetValue(_cache) as IDatabase;
        if (redisDatabase == null)
        {
            throw new InvalidOperationException("The redis database is not available.");
        }

        var ttl = redisDatabase.KeyTimeToLive(cacheKey);
        if (ttl == null)
        {
            return 0; // Key not found in the internal collection
        }

        return (int)ttl!.Value.TotalSeconds;
    }
}
