using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// WARNING! This api will directly send a request to the Tencent Server.
/// </summary>
/// <remarks>
/// Notice: You should conside to use <see also="IWeixinAccessTokenApi"/>, and enable a cache provider for your access token.
/// </remarks>
public interface IWeixinAccessTokenDirectApi
{
    /// <summary>
    /// 获取微信公众号全局接口调用凭证(access_token)。
    /// </summary>
    /// <returns>微信公众号全局接口调用凭证(access_token)</returns>
    Task<WeixinAccessTokenJson> GetTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取微信公众号全局接口调用凭证(access_token)。
    /// </summary>
    /// <returns>微信公众号全局接口调用凭证(access_token)</returns>
    WeixinAccessTokenJson GetToken();

    /// <summary>
    /// 强制刷新微信公众号全局接口调用凭证(access_token)。
    /// </summary>
    /// <returns>微信公众号全局接口调用凭证(access_token)</returns>
    Task<WeixinAccessTokenJson> GetAndRefreshTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 强制刷新微信公众号全局接口调用凭证(access_token)。
    /// </summary>
    /// <returns>微信公众号全局接口调用凭证(access_token)</returns>
    WeixinAccessTokenJson GetAndRefreshToken();
}