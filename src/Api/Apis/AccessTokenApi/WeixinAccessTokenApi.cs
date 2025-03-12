using System;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinAccessTokenApi : IWeixinAccessTokenApi
{
    private readonly WeixinAccessTokenDirectApi _api;
    private readonly IWeixinCacheProvider<WeixinAccessTokenJson> _cache;

    public string AppId { get => _api.Options.AppId; }

    public WeixinAccessTokenApi(WeixinAccessTokenDirectApi api, IWeixinCacheProvider<WeixinAccessTokenJson> cacheProvider)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _cache = cacheProvider ?? throw new ArgumentException(nameof(cacheProvider));
    }

    public async Task<WeixinAccessTokenJson> GetTokenAsync(bool forceRenew, CancellationToken cancellationToken = default)
    {
        if (_cache == null) // We allow the cache provider be null
        {
            var json = await FetchTokenAsync(cancellationToken);
            return json;
        }

        if (forceRenew)
        {
            _cache.Remove(AppId);
            var json = await FetchTokenAsync(cancellationToken);
            _cache.Replace(AppId, json);
            return json;
        }
        else
        {
            var accessToken = _cache.Get(AppId);
            if (string.IsNullOrEmpty(accessToken?.AccessToken))
            {
                var json = await FetchTokenAsync(cancellationToken);
                _cache.Replace(AppId, json);
                accessToken = json;
            }
            return accessToken;
        }
    }

    public Task<WeixinAccessTokenJson> GetTokenAsync(CancellationToken cancellationToken = default) => GetTokenAsync(false, cancellationToken);
    public WeixinAccessTokenJson GetToken() => Task.Run(async () => await GetTokenAsync()).Result;
    public WeixinAccessTokenJson GetToken(bool forceRenew) => Task.Run(async () => await GetTokenAsync(forceRenew)).Result;

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
