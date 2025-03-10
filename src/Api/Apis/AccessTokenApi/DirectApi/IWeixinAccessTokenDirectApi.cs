using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// WARNING! <see cref="IWeixinAccessTokenDirectApi"/> will directly send a request to the Tencent Server to fetch a new access token each time!
/// </summary>
/// <remarks>
// Notice: We have removed IWeixinAccessTokenDirectApi from Dependency Injection (DI) because it fetches a new access token each time it is called.
// You should conside to use <see also="IWeixinAccessTokenApi"/>, and enable a cache provider for your access token.</remarks>
public interface IWeixinAccessTokenDirectApi
{
    /// <summary>
    /// 获取新的微信API访问凭证。
    /// </summary>
    /// <returns>微信API访问凭证</returns>
    Task<WeixinAccessTokenJson> GetTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取新的微信API访问凭证。
    /// </summary>
    /// <returns>微信API访问凭证</returns>
    WeixinAccessTokenJson GetToken();
}