using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Use IDatabase direct access, get expiration from TTL in redis server.
/// </summary>
/// <remarks>You should use this implementation if you can, instead of <see cref="WeixinRedisCacheProvider{T}"/>.</remarks>
/// <seealso cref="WeixinRedisCacheProvider"/>
public class WeixinRedisCacheProvider : IWeixinCacheProvider
{
    private string GenerateCacheKey<T>(string appId) { return $"WX_{typeof(T).Name}_{appId}"; }

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

    public WeixinRedisCacheProvider(IOptions<RedisCacheOptions> optionsAccessor)
    {
        _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        _cache = GetDatabase() ?? throw new ArgumentNullException(nameof(optionsAccessor));
    }

    public T Get<T>(string appId) where T : IWeixinExpirableValue, new()
        => GetAsync<T>(appId).ConfigureAwait(false).GetAwaiter().GetResult();

    public async Task<T> GetAsync<T>(string appId, CancellationToken cancellationToken = default) where T : IWeixinExpirableValue, new()
    {
        var cacheKey = GenerateCacheKey<T>(appId);
        var expirableValue = new T();
        var value = await GetDatabase()?.StringGetAsync(cacheKey);
        expirableValue.Value = value;
        if (!string.IsNullOrEmpty(value))
        {
            expirableValue.ExpiresIn = ((int?)(await GetDatabase()?.KeyTimeToLiveAsync(cacheKey))?.TotalSeconds) ?? 0;
            if (expirableValue.Succeeded) return expirableValue;
        }
        return expirableValue;
    }

    public void Remove<T>(string appId)
    {
        var cacheKey = GenerateCacheKey<T>(appId);
        GetDatabase()?.KeyDeleteAsync(cacheKey);
    }

    public bool Replace<T>(string appId, T expirableValue) where T : IWeixinExpirableValue
        => ReplaceAsync(appId, expirableValue).ConfigureAwait(false).GetAwaiter().GetResult();

    public async Task<bool> ReplaceAsync<T>(string appId, T expirableValue, CancellationToken cancellationToken = default) where T : IWeixinExpirableValue
    {
        var cacheKey = GenerateCacheKey<T>(appId);
        await GetDatabase()?.StringSetAsync(cacheKey, expirableValue.Value, TimeSpan.FromSeconds(expirableValue.ExpiresIn - 1));

        // To ensure value stored
        var storedValue = await GetDatabase()?.StringGetAsync(cacheKey);
        return storedValue == expirableValue.Value;
    }
}

/// <summary>
/// Get and replace the whole object T as Json.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>You better not use expiration property in object T (if exists), because its value won't change at all.</remarks>
/// <seealso cref="WeixinRedisCacheProvider"/>
public class WeixinRedisCacheProvider<T> : IWeixinCacheProvider<T>
    where T : new()
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
        var bytes = await _cache.GetAsync(cacheKey);
        return (bytes == null) ? default : JsonSerializer.Deserialize<T>(bytes, GetJsonSerializerOptions());
    }

    public void Remove(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);
        _cache.Remove(cacheKey);
    }

    public bool Replace(string appId, T json, TimeSpan expiresIn)
        => Task.Run(async () => await ReplaceAsync(appId, json, expiresIn)).Result;

    public async Task<bool> ReplaceAsync(string appId, T obj, TimeSpan expiresIn, CancellationToken cancellationToken = default)
    {
        var entryOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiresIn
        };
        var cacheKey = GenerateCacheKey(appId);
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj, GetJsonSerializerOptions()));
        await _cache.SetAsync(cacheKey, bytes, entryOptions);

        // To ensure the value stored
        var storedJson = await _cache.GetAsync(cacheKey, cancellationToken);
        return storedJson != null;
    }

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            WriteIndented = true,
            AllowTrailingCommas = true,
#if NET5_0_OR_GREATER
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
#else
            IgnoreNullValues = true
#endif
        };
    }
}
