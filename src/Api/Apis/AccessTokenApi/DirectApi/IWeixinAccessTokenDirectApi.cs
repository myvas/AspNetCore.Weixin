using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// WARNING: <see cref="IWeixinAccessTokenDirectApi"/> will directly call the Tencent Server to get a new access token each time!
/// You should conside to use <see also="IWeixinAccessTokenApi"/>, and enable a cache provider for your access token.
/// </summary>
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