using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinAccessTokenApi //: IWeixinAccessTokenDirectApi
{
    /// <summary>
    /// 获取微信API访问凭证。
    /// </summary>
    /// <param name="forceRenew">当forceRenew=true时，每次调用都获取新的访问凭证; 当其为false时，功能与<see cref="GetTokenAsync()"/>等效。</param>
    /// <returns>微信API访问凭证</returns>
    Task<WeixinAccessTokenJson> GetTokenAsync(bool forceRenew, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// 获取微信API访问凭证。
    /// </summary>
    /// <param name="forceRenew">当forceRenew=true时，每次调用都获取新的访问凭证; 当其为false时，功能与<see cref="GetTicket()"/>等效。</param>
    /// <returns>微信API访问凭证</returns>
    WeixinAccessTokenJson GetToken(bool forceRenew);
    
    /// <summary>
    /// 获取微信API访问凭证。仅在需要时调用微信API接口，即：若凭证尚在有效期内，则直接取回上一次得到的凭证。
    /// </summary>
    /// <returns>微信API访问凭证</returns>
    Task<WeixinAccessTokenJson> GetTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取微信API访问凭证。仅在需要时调用微信API接口，即：若凭证尚在有效期内，则直接取回上一次得到的凭证。
    /// </summary>
    /// <returns>微信API访问凭证</returns>
    WeixinAccessTokenJson GetToken();
}