using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// WARNING: <see cref="IWeixinJsapiTicketDirectApi"/> will directly call the Tencent Server to get a new access token each time!
/// You should use <see cref="IWeixinJsapiTicketApi"/>, and enable a cache provider for JSAPI Ticket.
/// </summary>
public interface IWeixinJsapiTicketDirectApi
{
    /// <summary>
    /// 获取新的微信JSSDK访问凭证。
    /// </summary>
    /// <returns>微信JSSDK访问凭证</returns>
    Task<WeixinJsapiTicketJson> GetTicketAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取新的微信JSSDK访问凭证。
    /// </summary>
    /// <returns>微信JSSDK访问凭证</returns>
    WeixinJsapiTicketJson GetTicket();
}