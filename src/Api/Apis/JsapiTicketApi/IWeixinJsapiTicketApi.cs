using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinJsapiTicketApi //: IWeixinJsapiTicketUnsafeApi
{
    /// <summary>
    /// 获取微信JSSDK访问凭证。
    /// </summary>
    /// <param name="forceRenew">当forceRenew=true时，每次调用都获取新的访问凭证; 当其为false时，功能与<see cref="GetTicketAsync()"/>等效。</param>
    /// <returns>微信JSSDK访问凭证</returns>
    Task<WeixinJsapiTicketJson> GetTicketAsync(bool forceRenew, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取微信JSSDK访问凭证。
    /// </summary>
    /// <param name="forceRenew">当forceRenew=true时，每次调用都获取新的访问凭证; 当其为false时，功能与<see cref="GetTicket()"/>等效。</param>
    /// <returns>微信JSSDK访问凭证</returns>
    WeixinJsapiTicketJson GetTicket(bool forceRenew);

    /// <summary>
    /// 获取微信JSSDK访问凭证。仅在需要时调用微信JSSDK接口，即：若凭证尚在有效期内，则直接取回上一次得到的凭证。
    /// </summary>
    /// <returns>微信JSSDK访问凭证</returns>
    Task<WeixinJsapiTicketJson> GetTicketAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取微信JSSDK访问凭证。仅在需要时调用微信JSSDK接口，即：若凭证尚在有效期内，则直接取回上一次得到的凭证。
    /// </summary>
    /// <returns>微信JSSDK访问凭证</returns>
    WeixinJsapiTicketJson GetTicket();
}