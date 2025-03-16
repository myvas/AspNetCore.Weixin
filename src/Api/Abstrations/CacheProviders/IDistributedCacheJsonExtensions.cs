using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

public static class IDistributedCacheGenericExtensions
{
    public static Task SetAsJsonAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, GetJsonSerializerOptions()));
        return cache.SetAsync(key, bytes, options);
    }

    public static async Task<T> GetFromJsonAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default)
    {
        var bytes = await cache.GetAsync(key);
        return (bytes == null) ? default : JsonSerializer.Deserialize<T>(bytes, GetJsonSerializerOptions());
    }

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            WriteIndented = true,
            AllowTrailingCommas = true,
            IgnoreNullValues = true
        };
    }
}