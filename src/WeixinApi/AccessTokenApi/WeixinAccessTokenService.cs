using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinAccessTokenService : IWeixinAccessToken
    {
        private readonly WeixinApiOptions _options;
        private readonly WeixinAccessTokenApi _api;
        private readonly IWeixinAccessTokenCacheProvider _cache;

        public WeixinAccessTokenService(IOptions<WeixinApiOptions> optionsAccessor, WeixinAccessTokenApi api, IWeixinAccessTokenCacheProvider cacheProvider)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _cache = cacheProvider;// ?? throw new ArgumentNullException(nameof(cacheProvider));
        }

        public async Task<string> GetTokenAsync(bool forceRenew, CancellationToken cancellationToken = default)
        {
            var appId = _options.AppId;

            if (_cache == null) // We allow the cache provider be null
            {
                var json = await FetchTokenAsync(cancellationToken);
                return json.access_token;
            }

            if (forceRenew)
            {
                _cache.Remove(appId);
                var json = await FetchTokenAsync(cancellationToken);
                _cache.Replace(appId, json);
                return json.access_token;
            }
            else
            {
                var accessToken = _cache.Get(appId);
                if (string.IsNullOrEmpty(accessToken))
                {
                    var json = await FetchTokenAsync(cancellationToken);
                    _cache.Replace(appId, json);
                    accessToken = json.access_token;
                }
                return accessToken;
            }
        }

        public Task<string> GetTokenAsync(CancellationToken cancellationToken = default) => GetTokenAsync(false, cancellationToken);
        public string GetToken() => GetTokenAsync().Result;
        public string GetToken(bool forceRenew) => GetTokenAsync(forceRenew).Result;

        #region private methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private Task<WeixinAccessTokenJson> FetchTokenAsync(CancellationToken cancellationToken = default)
        {
            //var appId = _options.AppId;
            //var appSecret = _options.AppSecret;
            return _api.GetTokenAsync(cancellationToken);
        }
        #endregion
    }
}
