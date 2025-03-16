using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Myvas.AspNetCore.Weixin;

public class WeixinExpirationMemoryCacheProvider<T> : IWeixinCacheProvider<T>
    where T : IWeixinExpirableValue, new()
{
    //private static readonly string CachePrefix = Guid.NewGuid().ToString("N");
    //private static readonly string CachePrefix = "WX_A_TOKEN";
    // typeof(T).Name.GetHashCode() has different value on different dotnet runtimes, so avoid to use it!
    //private static readonly string CachePrefix = "WX_" + typeof(T).Name.GetHashCode().ToString("X");
    private static readonly string CachePrefix = "WX_" + typeof(T).Name;
    private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

    private readonly IMemoryCache _cache;

    public WeixinExpirationMemoryCacheProvider(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T Get(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);

        var result = new T();
        if (_cache.TryGetValue(cacheKey, out string value))
        {
            if (string.IsNullOrEmpty(value))
            {
                return result;
            }
            result.Value = value;
            result.ExpiresIn = GetAbsoluteExpiration(cacheKey);

            return result.Validate() ? result : default;
        }

        return default;
    }

    public void Remove(string appId)
    {
        var cacheKey = GenerateCacheKey(appId);
        _cache.Remove(cacheKey);
    }

    public bool Replace(string appId, T expirableValue)
    {
        var cacheKey = GenerateCacheKey(appId);
        // Cut off 1s for this Set operation
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(expirableValue.ExpiresIn - 1));
        _cache.Set(cacheKey, expirableValue.Value, cacheEntryOptions);

        // To ensure the value stored        
        if (_cache.TryGetValue(cacheKey, out string storedValue))
            return storedValue == expirableValue.Value;
        return false;
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
        var fields = memoryCacheType.GetFields(
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance |
            BindingFlags.Static
        );

#if DEBUG
        Debug.WriteLine($"Fields of {memoryCacheType}:");
        foreach (var field in fields)
        {
            Debug.WriteLine($"{field.Name} ({field.FieldType})");
        }
#endif
        IDictionary entriesCollection = null;
        if (fields.FirstOrDefault(x => x.Name == "_coherentState") != null)
        {
            var coherentStateField = memoryCacheType.GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);
            if (coherentStateField == null)
            {
                throw new InvalidOperationException($"Unable to access the internal _coherentState collection of {memoryCacheType}.");
            }

            // Get the value of the _coherentState field
            var coherentState = coherentStateField.GetValue(_cache);
            if (coherentState == null)
            {
                throw new InvalidOperationException("The _coherentState field is null.");
            }

            // Use reflection to access the EntriesCollection property of CoherentState
            var coherentStateType = coherentState.GetType();
            // net7.0
            var entriesCollectionProperty = coherentStateType.GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            if (entriesCollectionProperty == null)
            {
                // net?
                entriesCollectionProperty = coherentStateType.GetProperty("StringEntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
                if (entriesCollectionProperty == null)
                    throw new InvalidOperationException("Could not find the EntriesCollection and StringEntriesCollection property.");
            }

            // Get the value of the EntriesCollection property
            entriesCollection = entriesCollectionProperty.GetValue(coherentState) as IDictionary;
            if (entriesCollection == null)
            {
                throw new InvalidOperationException("The StringEntriesCollection is null or not a dictionary.");
            }
        }
        else if (fields.FirstOrDefault(x => x.Name == "_entries") != null)
        {
            //net5.0
            var entriesCollectionField = memoryCacheType.GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance);
            if (entriesCollectionField == null)
            {
                throw new InvalidOperationException($"Unable to access the internal _entries collection of {memoryCacheType}.");
            }

            entriesCollection = entriesCollectionField.GetValue(_cache) as IDictionary;
            if (entriesCollection == null)
            {
                throw new InvalidOperationException("The _entries is null or not a dictionary.");
            }
        }
        else if (fields.FirstOrDefault(x => x.Name == "_stringKeyEntries") != null)
        {
            //net6.0
            var entriesCollectionField = memoryCacheType.GetField("_stringKeyEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            if (entriesCollectionField == null)
            {
                throw new InvalidOperationException($"Unable to access the internal _stringKeyEntries collection of {memoryCacheType}.");
            }

            entriesCollection = entriesCollectionField.GetValue(_cache) as IDictionary;
            if (entriesCollection == null)
            {
                throw new InvalidOperationException("The _stringKeyEntries is null or not a dictionary.");
            }
        }

        // Find the cache entry associated with the key
        if (!entriesCollection!.Contains(cacheKey))
        {
            return 0; // Key not found in the internal collection
        }

        var cacheEntry = entriesCollection[cacheKey];
        var cacheEntryType = cacheEntry.GetType();

        // net6.0 (PUBLIC)
        var absolutionExpirationProperty = cacheEntryType.GetProperty("AbsoluteExpiration", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        DateTimeOffset? absoluteExpiration = absolutionExpirationProperty?.GetValue(cacheEntry) as DateTimeOffset?;
        if (!absoluteExpiration.HasValue)
        {
            //net5.0
            absolutionExpirationProperty = cacheEntryType.GetProperty("_absoluteExpiration", BindingFlags.NonPublic | BindingFlags.Instance);
            absoluteExpiration = absolutionExpirationProperty?.GetValue(cacheEntry) as DateTimeOffset?;
            if (!absoluteExpiration.HasValue)
            {
                var absoluteExpirationTicksProperty = cacheEntryType.GetProperty("AbsoluteExpirationTicks", BindingFlags.NonPublic | BindingFlags.Instance);
                var absoluteExpirationTicks = absoluteExpirationTicksProperty?.GetValue(cacheEntry) as long?;
                if (absoluteExpirationTicks.HasValue)
                {
                    absoluteExpiration = new DateTimeOffset(absoluteExpirationTicks!.Value, TimeSpan.Zero);
                }
                else
                {
                    // Access the SlidingExpiration property
                    var slidingExpirationProperty = cacheEntryType.GetProperty("SlidingExpiration", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    var slidingExpiration = slidingExpirationProperty?.GetValue(cacheEntry) as TimeSpan?;
                    if (slidingExpiration.HasValue)
                    {
                        // Access the LastAccessed property
                        var lastAccessedProperty = cacheEntryType.GetProperty("LastAccessed", BindingFlags.NonPublic | BindingFlags.Instance);
                        var lastAccessed = lastAccessedProperty?.GetValue(cacheEntry) as DateTimeOffset?;
                        if (lastAccessed.HasValue)
                        {
                            absoluteExpiration = lastAccessed.Value.Add(slidingExpiration.Value);
                        }
                    }
                }
            }
        }
        if (absoluteExpiration.HasValue)
        {
            var result = (int)absoluteExpiration!.Value.Subtract(DateTimeOffset.Now).TotalSeconds;
            // Cut off 1 second for safe return
            return result - 1;
        }

        return 0;
    }
}
