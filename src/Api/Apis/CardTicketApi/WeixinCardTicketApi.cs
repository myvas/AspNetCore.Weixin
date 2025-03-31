using System;
using System.Threading.Tasks;
using System.Threading;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Get Weixin Card Ticket
/// </summary>
public class WeixinCardTicketApi : IWeixinCardTicketApi
{
    private readonly WeixinCardTicketDirectApi _api;
    private readonly IWeixinCacheProvider _cache;

    public string AppId { get => _api.Options.AppId; }

    public WeixinCardTicketApi(WeixinCardTicketDirectApi api, IWeixinCacheProvider cacheProvider)
    {
        _api = api ?? throw new ArgumentNullException(nameof(api));
        _cache = cacheProvider ?? throw new ArgumentException(nameof(cacheProvider));
    }

    public async Task<WeixinCardTicketJson> GetTicketAsync(bool forceRenew, CancellationToken cancellationToken = default)
    {
        if (_cache == null) // We allow the cache provider be null
        {
            var json = await FetchTicketAsync(cancellationToken);
            return json;
        }

        if (forceRenew)
        {
            _cache.Remove<WeixinCardTicketJson>(AppId);
            var json = await FetchTicketAsync(cancellationToken);
            _cache.Replace(AppId, json);
            return json;
        }
        else
        {
            var cardTicket = _cache.Get<WeixinCardTicketJson>(AppId);
            if (cardTicket==null || !cardTicket!.Succeeded)
            {
                var json = await FetchTicketAsync(cancellationToken);
                _cache.Replace(AppId, json);
                cardTicket = json;
            }
            return cardTicket;
        }
    }

    public Task<WeixinCardTicketJson> GetTicketAsync(CancellationToken cancellationToken = default) => GetTicketAsync(false, cancellationToken);
    public WeixinCardTicketJson GetTicket() => GetTicketAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    public WeixinCardTicketJson GetTicket(bool forceRenew) => GetTicketAsync(forceRenew).ConfigureAwait(false).GetAwaiter().GetResult();

    #region private methods
    /// <summary>
    /// Call <see cref="IWeixinCardTicketDirectApi.GetTicketAsync"/> to get a ticket.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private Task<WeixinCardTicketJson> FetchTicketAsync(CancellationToken cancellationToken = default)
    {
        return _api.GetTicketAsync(cancellationToken);
    }
    #endregion
}
