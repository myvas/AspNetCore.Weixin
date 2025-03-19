using System;
using System.Threading.Tasks;
using System.Threading;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 获取微信Card Ticket数据服务接口
/// </summary>
public class WeixinCardTicketApi : IWeixinCardTicketApi
{
    private readonly WeixinCardTicketDirectApi _api;
    private readonly IWeixinCacheProvider<WeixinCardTicketJson> _cache;

    public string AppId { get => _api.Options.AppId; }

    public WeixinCardTicketApi(WeixinCardTicketDirectApi api, IWeixinCacheProvider<WeixinCardTicketJson> cacheProvider)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _cache = cacheProvider ?? throw new ArgumentException(nameof(cacheProvider));
    }

    public async Task<WeixinCardTicketJson> GetTicketAsync(bool forceRenew, CancellationToken cancellationToken = default)
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
            if (string.IsNullOrEmpty(accessToken?.Ticket))
            {
                var json = await FetchTokenAsync(cancellationToken);
                _cache.Replace(AppId, json);
                accessToken = json;
            }
            return accessToken;
        }
    }

    public Task<WeixinCardTicketJson> GetTicketAsync(CancellationToken cancellationToken = default) => GetTicketAsync(false, cancellationToken);
    public WeixinCardTicketJson GetTicket() => Task.Run(async () => await GetTicketAsync()).Result;
    public WeixinCardTicketJson GetTicket(bool forceRenew) => Task.Run(async () => await GetTicketAsync(forceRenew)).Result;

    #region private methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private Task<WeixinCardTicketJson> FetchTokenAsync(CancellationToken cancellationToken = default)
    {
        //var appId = _options.AppId;
        //var appSecret = _options.AppSecret;
        return _api.GetTicketAsync(cancellationToken);
    }
    #endregion
}
