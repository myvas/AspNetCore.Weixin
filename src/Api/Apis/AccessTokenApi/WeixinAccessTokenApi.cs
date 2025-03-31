using System;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinAccessTokenApi : IWeixinAccessTokenApi
{
    private readonly WeixinAccessTokenDirectApi _api;
    private readonly IWeixinCacheProvider _cache;

    public string AppId { get => _api.Options.AppId; }

    public WeixinAccessTokenApi(WeixinAccessTokenDirectApi api, IWeixinCacheProvider cacheProvider)
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
            _cache.Remove<WeixinAccessTokenJson>(AppId);
            var json = await FetchTokenAsync(cancellationToken);
            _cache.Replace(AppId, json);
            return json;
        }
        else
        {
            var accessToken = _cache.Get<WeixinAccessTokenJson>(AppId);
            if (accessToken == null || !accessToken!.Succeeded)
            {
                var json = await FetchTokenAsync(cancellationToken);
                _cache.Replace(AppId, json);
                accessToken = json;
            }
            return accessToken;
        }
    }

    public Task<WeixinAccessTokenJson> GetTokenAsync(CancellationToken cancellationToken = default) => GetTokenAsync(false, cancellationToken);
    public WeixinAccessTokenJson GetToken() => GetTokenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    public WeixinAccessTokenJson GetToken(bool forceRenew) => GetTokenAsync(forceRenew).ConfigureAwait(false).GetAwaiter().GetResult();

    #region private methods
    /// <summary>
    /// Call <see cref="IWeixinAccessTokenDirectApi.GetTokenAsync"/> to get a token.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<WeixinAccessTokenJson> FetchTokenAsync(CancellationToken cancellationToken = default)
    {
        return await _api.GetTokenAsync(cancellationToken);
    }
    #endregion
}
