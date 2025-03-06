using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinAccessTokenMemoryCacheProvider : IWeixinAccessTokenCacheProvider
    {
        private static readonly string CachePrefix = Guid.NewGuid().ToString("N");
        private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

        private readonly IMemoryCache _cache;

        public WeixinAccessTokenMemoryCacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string Get(string appId)
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

        public void Replace(string appId, WeixinAccessTokenJson json)
        {
            var accessToken = json.access_token;
            var absoluteExpirationRelativeToNow = TimeSpan.FromSeconds(json.expires_in);
            var cacheKey = GenerateCacheKey(appId);
            _cache.Set(cacheKey, accessToken, absoluteExpirationRelativeToNow);
        }
    }
}
