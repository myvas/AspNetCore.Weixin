using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class AccessTokenService : IWeixinAccessToken
    {
        private readonly IWeixinAccessTokenCacheProvider _cache;
        private readonly WeixinAccessTokenOptions _options;
        private readonly AccessTokenApi _api;

        public AccessTokenService(IOptions<WeixinAccessTokenOptions> optionsAccessor, AccessTokenApi api, IWeixinAccessTokenCacheProvider cache)
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
                _cache.RemoveCachedAccessToken(appId);
                var json = await _api.GetTokenAsync(appId, appSecret);
                _cache.ReplaceCachedAccessToken(appId, json);
                return json.access_token;
            }
            else
            {
                var accessToken = _cache.TryGetCachedAccessToken(appId);
                if (string.IsNullOrEmpty(accessToken))
                {
                    var json = await _api.GetTokenAsync(appId, appSecret);
                    _cache.ReplaceCachedAccessToken(appId, json);
                    accessToken = json.access_token;
                }
                return accessToken;
            }
        }

        public Task<string> GetTokenAsync() => GetTokenAsync(false);
        public string GetToken() => GetTokenAsync().Result;
        public string GetToken(bool forceRenew) => GetTokenAsync(forceRenew).Result;
    }
}
