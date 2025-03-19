using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// WARNING: <see cref="IWeixinCardTicketDirectApi"/> will directly call the Tencent Server to get a new access token each time!
/// You should use <see cref="IWeixinCardTicketApi"/>, and enable a cache provider for Weixin Card.
/// </summary>
public interface IWeixinCardTicketDirectApi
{
    /// <summary>
    /// 获取新的微信Card访问凭证。
    /// </summary>
    /// <returns>微信Card访问凭证</returns>
    Task<WeixinCardTicketJson> GetTicketAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取新的微信Card访问凭证。
    /// </summary>
    /// <returns>微信Card访问凭证</returns>
    WeixinCardTicketJson GetTicket();
}
