using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 获取微信服务器地址
/// </summary>
public class WeixinCommonApi : WeixinSecureApiClient, IWeixinCommonApi
{
    public WeixinCommonApi(IOptions<WeixinOptions> optionsAccessor, IWeixinAccessTokenApi tokenProvider) : base(optionsAccessor, tokenProvider)
    {
    }

    /// <summary>
    /// 获取微信callback IP地址
    /// https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Get_the_WeChat_server_IP_address.html
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<WeixinIpResponseJson> GetCallbackIpsAsync(CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/getcallbackip?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        var result = await SecureGetFromJsonAsync<WeixinIpResponseJson>(url);
        if (result.Succeeded)
            return result;
        else
            throw new WeixinException(result);
    }

    /// <summary>
    /// 获取微信API接口 IP地址
    /// https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Get_the_WeChat_server_IP_address.html
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<WeixinIpResponseJson> GetTencentServerIpsAsync(CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/get_api_domain_ip?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        var result = await SecureGetFromJsonAsync<WeixinIpResponseJson>(url);
        if (result.Succeeded)
            return result;
        else
            throw new WeixinException(result);
    }


    /// <summary>
    /// 网络检测
    /// https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Network_Detection.html
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<WeixinCheckNetworkResponseJson> CheckNetworkAsync(WeixinCheckNetworkRequestJson data, CancellationToken cancellationToken = default)
    {
        var pathAndQuery = "/cgi-bin/callback/check?access_token=ACCESS_TOKEN";
        var url = Options?.BuildWeixinApiUrl(pathAndQuery);

        //var data = new
        //{
        //    action = "all",
        //    check_operator = "DEFAULT"
        //};
        var result = await SecurePostAsJsonAsync<WeixinCheckNetworkRequestJson, WeixinCheckNetworkResponseJson>(url, data);
        if (result.Succeeded)
            return result;
        else
            throw new WeixinException(result);
    }

    public Task<WeixinCheckNetworkResponseJson> CheckNetworkAsync(string action, string check_operator, CancellationToken cancellationToken = default)
        => CheckNetworkAsync(new WeixinCheckNetworkRequestJson(action, check_operator), cancellationToken);
}
