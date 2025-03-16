using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 获取微信凭证数据服务接口
/// </summary>
public sealed class WeixinAccessTokenDirectApi : WeixinApiClient, IWeixinAccessTokenDirectApi
{
    public WeixinAccessTokenDirectApi(IOptions<WeixinOptions> optionsAccessor)
        : base(optionsAccessor)
    {
    }

    /// <summary>
    /// 获取稳定版接口调用凭据，可供微信公众号全局后台接口调用时使用。
    /// </summary>
    /// <param name="forceRefresh">
    /// <p>true: 强制刷新模式，该模式调用次数限制为每天20次，调用后将立即作废旧凭证启用新凭证（但再次调用至少间隔30秒，则不会刷新凭证）。</p>
    /// <p>false: 普通模式，该模式下有效期内重复调用该接口不会更新凭证，且获得的凭证至少5分钟内可用（除非调用强制刷新），因为平台会提前5分钟产生新凭证，此时两个凭证均可用。</p>
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>微信公众号全局接口调用凭证(access_token)</returns>
    /// <remarks>
    /// 与<see cref="GetNonstableTokenAsync"/>获取的调用凭证完全隔离，互不影响。
    /// </remakrs>
    /// <seealso href="https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/getStableAccessToken.html">微信官方文档</seealso>
    private async Task<WeixinAccessTokenJson> GetStableTokenAsync(bool forceRefresh, CancellationToken cancellationToken = default)
    {
        var url = Options?.BuildWeixinApiUrl("/cgi-bin/stable_token");

        var oBody = new
        {
            grant_type = "client_credential",
            appid = Options.AppId,
            secret = Options.AppSecret,
            force_refresh = forceRefresh
        };
        var jsonBody = JsonSerializer.Serialize(oBody);

        try
        {
            var result = await PostContentAsJsonAsync<WeixinAccessTokenJson>(url, new StringContent(jsonBody), cancellationToken);
            return result.Succeeded ? result : throw new WeixinAccessTokenException(result);
        }
        catch (WeixinAccessTokenException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw WeixinAccessTokenErrors.GenericError(ex);
        }
    }

    /// <summary>
    /// 获取微信公众号全局接口调用凭证(access_token)。
    /// </summary>
    /// <returns>微信公众号全局接口调用凭证(access_token)</returns>
    /// <remarks>至少5分钟内可用，除非用户调用<see cref="GetAndRefreshTokenAsync"/>强制刷新。</remarks>
    public Task<WeixinAccessTokenJson> GetTokenAsync(CancellationToken cancellationToken = default) => GetStableTokenAsync(false);

    /// <summary>
    /// 强制刷新微信公众号全局接口调用凭证(access_token)。
    /// </summary>
    /// <returns>微信公众号全局接口调用凭证(access_token)</returns>
    /// <remarks>注意：本接口调用限制为20次/日。若在30秒内重复调用不会有效作废旧凭证！</remarks>
    public Task<WeixinAccessTokenJson> GetAndRefreshTokenAsync(CancellationToken cancellationToken = default) => GetStableTokenAsync(true);

    /// <summary>
    /// 获取微信公众号全局接口调用凭证(access_token)。
    /// </summary>
    /// <returns>微信公众号全局接口调用凭证(access_token)</returns>
    /// <remarks>至少5分钟内可用，除非用户调用<see cref="GetAndRefreshTokenAsync"/>强制刷新。</remarks>
    public WeixinAccessTokenJson GetToken() => Task.Run(async () => await GetTokenAsync()).Result;

    /// <summary>
    /// 强制刷新微信公众号全局接口调用凭证(access_token)。
    /// </summary>
    /// <returns>微信公众号全局接口调用凭证(access_token)</returns>
    /// <remarks>注意：本接口调用限制为20次/日。若在30秒内重复调用不会有效作废旧凭证！</remarks>
    public WeixinAccessTokenJson GetAndRefreshToken() => Task.Run(async () => await GetAndRefreshTokenAsync()).Result;

    #region This code is deprecated but kept here for reference and memorization purposes.
    /// <summary>
    /// 获取微信凭证
    /// <para>access_token是公众号的全局唯一票据，公众号调用各接口时都需使用access_token。正常情况下access_token有效期为7200秒，重复获取将导致上次获取的access_token失效。由于获取access_token的api调用次数非常有限，建议开发者全局存储与更新access_token，频繁刷新access_token会导致api调用受限，影响自身业务。</para>
    /// <para>请开发者注意，由于技术升级，公众平台的开发接口的access_token长度将增长，其存储至少要保留512个字符空间。此修改将在1个月后生效，请开发者尽快修改兼容。</para>
    /// <para>公众号可以使用AppID和AppSecret调用本接口来获取access_token。AppID和AppSecret可在开发模式中获得（需要已经成为开发者，且帐号没有异常状态）。注意调用所有微信接口时均需使用https协议。</para>
    /// </summary>
    /// <param name="appid">开发者ID之AppId。
    /// <para>例如：<code>wxd8fb1eb9ecf48d15</code></para></param>
    /// <param name="secret">开发者ID之AppSecret。
    /// <para>例如：<code>b7ffe201d9f7db183b8827ebe789aa88</code></para></param>
    /// <param name="grantType">默认为：<code>client_credential</code></param>
    /// <returns>微信凭证数据
    /// <para>正常情况下，微信会返回下述JSON数据包给公众号。例如：</para>
    /// <code>{"access_token":"ACCESS_TOKEN","expires_in":7200}</code>
    /// </returns>
    /// <exception cref="WeixinException">
    /// <para>错误时微信会返回错误码等信息，JSON数据包示例如下（该示例为AppID无效错误）:</para>
    /// <code>{"errcode":40013,"errmsg":"invalid appid"}</code>
    /// </exception>
    /// <remarks>    
    /// </remarks>
    /// <seealso href="https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Get_access_token.html">微信官方文档</seealso>
    /// <seealso href="https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421140183">微信官方文档</seealso>
    private async Task<WeixinAccessTokenJson> GetNonstableTokenAsync(CancellationToken cancellationToken = default)
    {
        var url = Options?.BuildWeixinApiUrl("/cgi-bin/token");

        var query = new QueryBuilder
        {
            { "grant_type", "client_credential" },
            { "appid", Options.AppId },
            { "secret", Options.AppSecret }
        };
        var requestUri = url + query.ToString();

        try
        {
            var result = await GetFromJsonAsync<WeixinAccessTokenJson>(requestUri, cancellationToken);
            return result.Succeeded ? result : throw new WeixinAccessTokenException(result);
        }
        catch (WeixinAccessTokenException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw WeixinAccessTokenErrors.GenericError(ex);
        }
    }
    #endregion
}
