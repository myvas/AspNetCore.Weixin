using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    public class MemoryAccessTokenCacheProvider : IWeixinAccessTokenCacheProvider
    {
        private static string CachePrefix = Guid.NewGuid().ToString("N");
        private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

        private readonly IMemoryCache _cache;
        private WeixinOptions _options;

        public MemoryAccessTokenCacheProvider(IMemoryCache cache, IOptions<WeixinOptions> optionsAccessor)
        {
            _cache = cache;
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        }

        public string TryGetCachedAccessToken(string appId)
        {
            var cacheKey = GenerateCacheKey(appId);
            if (_cache.TryGetValue(cacheKey, out string accessToken))
            {
                return accessToken;
            }
            else
            {
                return "";
            }
        }

        public void RemoveCachedAccessToken(string appId)
        {
            var cacheKey = GenerateCacheKey(appId);
            _cache.Remove(cacheKey);
        }

        public void ReplaceCachedAccessToken(string appId, string accessToken, TimeSpan absoluteExpirationRelativeToNow)
        {
            var cacheKey = GenerateCacheKey(appId);
            _cache.Set(cacheKey, accessToken, absoluteExpirationRelativeToNow);
        }

        public void ReplaceCachedAccessToken(string appId, WeixinAccessTokenJson json)
        {
            var accessToken = json.AccessToken;
            var absoluteExpirationRelativeToNow = TimeSpan.FromSeconds(json.ExpiresIn);
            var cacheKey = GenerateCacheKey(appId);
            _cache.Set(cacheKey, accessToken, absoluteExpirationRelativeToNow);
        }
    }
}
