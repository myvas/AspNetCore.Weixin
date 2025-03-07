using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class AccessTokenService : IWeixinAccessToken
    {
        private string GenerateCacheKey(string appId) { return $"accesstoken_{appId}"; }

        private readonly IDistributedCache _cache;
        private readonly WeixinOptions _options;
        private readonly AccessTokenApi _api;

        public AccessTokenService(
            IOptions<WeixinOptions> optionsAccessor,
            AccessTokenApi api,
            IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _api = api ?? throw new ArgumentNullException(nameof(api));
        }

        public async Task<string> GetTokenAsync(bool forceRenew)
        {
            var appId = _options.AppId;
            var appSecret = _options.AppSecret;
            var cacheKey = GenerateCacheKey(appId);
            if (forceRenew)
            {
                _cache.Remove(cacheKey);
            }

            {
                var accessToken = await _cache.GetStringAsync(cacheKey);
                if (string.IsNullOrEmpty(accessToken))
                {
                    var json = await _api.GetTokenAsync(appId, appSecret);
                    if (json.Succeeded)
                    {
                        accessToken = json.AccessToken;
                        var cacheEntryOptions = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(json.ExpiresIn));
                        await _cache.SetStringAsync(cacheKey, accessToken, cacheEntryOptions);
                    }
                }
                return accessToken;
            }
        }

        public Task<string> GetTokenAsync() => GetTokenAsync(false);

        public string GetToken() => GetTokenAsync()?.Result;
        public string GetToken(bool forceRenew) => GetTokenAsync(forceRenew)?.Result;
    }
}
