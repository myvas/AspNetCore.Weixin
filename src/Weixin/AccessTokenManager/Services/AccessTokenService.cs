using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class AccessTokenService : IWeixinAccessToken
    {
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
            if (forceRenew)
            {
                _cache.Remove(appId);
                var json = await _api.GetTokenAsync(appId, appSecret);
                if (json.Succeeded)
                {
                    var accessToken = json.AccessToken;
                    var cacheEntryOptions = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(json.ExpiresIn));
                    await _cache.SetStringAsync(appId, accessToken, cacheEntryOptions);
                    return accessToken;
                }
                return null;
            }
            else
            {
                var accessToken = await _cache.GetStringAsync(appId);
                if (string.IsNullOrEmpty(accessToken))
                {
                    var json = await _api.GetTokenAsync(appId, appSecret);
                    if (json.Succeeded)
                    {
                        accessToken = json.AccessToken;
                        var cacheEntryOptions = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(json.ExpiresIn));
                        await _cache.SetStringAsync(appId, accessToken, cacheEntryOptions);
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
