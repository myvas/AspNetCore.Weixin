using System;
using System.Threading.Tasks;
using System.Threading;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Get Weixin JsapiTicket
/// </summary>
public class WeixinJsapiTicketApi : IWeixinJsapiTicketApi
{
    private readonly WeixinJsapiTicketDirectApi _api;
    private readonly IWeixinCacheProvider _cache;

    public string AppId { get => _api.Options.AppId; }

    public WeixinJsapiTicketApi(WeixinJsapiTicketDirectApi api, IWeixinCacheProvider cacheProvider)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _cache = cacheProvider ?? throw new ArgumentException(nameof(cacheProvider));
    }

    public async Task<WeixinJsapiTicketJson> GetTicketAsync(bool forceRenew, CancellationToken cancellationToken = default)
    {
        if (_cache == null) // We allow the cache provider be null
        {
            var json = await FetchTicketAsync(cancellationToken);
            return json;
        }

        if (forceRenew)
        {
            _cache.Remove<WeixinJsapiTicketJson>(AppId);
            var json = await FetchTicketAsync(cancellationToken);
            _cache.Replace(AppId, json);
            return json;
        }
        else
        {
            var jsapiTicket = _cache.Get<WeixinJsapiTicketJson>(AppId);
            if (jsapiTicket == null || !jsapiTicket!.Succeeded)
            {
                var json = await FetchTicketAsync(cancellationToken);
                _cache.Replace(AppId, json);
                jsapiTicket = json;
            }
            return jsapiTicket;
        }
    }

    public Task<WeixinJsapiTicketJson> GetTicketAsync(CancellationToken cancellationToken = default) => GetTicketAsync(false, cancellationToken);
    public WeixinJsapiTicketJson GetTicket() => GetTicketAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    public WeixinJsapiTicketJson GetTicket(bool forceRenew) => GetTicketAsync(forceRenew).ConfigureAwait(false).GetAwaiter().GetResult();

    #region private methods
    /// <summary>
    /// Call <see cref="IWeixinJsapiTicketDirectApi.GetTicketAsync"/> to get a ticket.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<WeixinJsapiTicketJson> FetchTicketAsync(CancellationToken cancellationToken = default)
    {
        return await _api.GetTicketAsync(cancellationToken);
    }
    #endregion
}
