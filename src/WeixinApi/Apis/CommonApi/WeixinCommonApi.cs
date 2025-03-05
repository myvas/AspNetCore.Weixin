using System.Linq;
using System.Text;
using System.Globalization;
using Myvas.AspNetCore.Weixin;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;
using System;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 获取微信服务器地址
    /// </summary>
    public class WeixinCommonApi : SecureApiClient, IWeixinCommonApi
    {
        public WeixinCommonApi(IOptions<WeixinApiOptions> optionsAccessor, IWeixinAccessToken tokenProvider) : base(optionsAccessor, tokenProvider)
        {
        }

        /// <summary>
        /// 获取微信callback IP地址
        /// https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Get_the_WeChat_server_IP_address.html
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IpResponseJson> GetCallbackIpsAsync(CancellationToken cancellationToken = default)
        {
            var api = "https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token=ACCESS_TOKEN";
            var access_token = await _tokenProvider.GetTokenAsync(cancellationToken);
            api = api.Replace("ACCESS_TOKEN", access_token);

            var result = await GetFromJsonAsync<IpResponseJson>(api);
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
        public async Task<IpResponseJson> GetTencentServerIpsAsync(CancellationToken cancellationToken = default)
        {
            var api = "https://api.weixin.qq.com/cgi-bin/get_api_domain_ip?access_token=ACCESS_TOKEN";
            var access_token = await _tokenProvider.GetTokenAsync(cancellationToken);
            api = api.Replace("ACCESS_TOKEN", access_token);

            var result = await GetFromJsonAsync<IpResponseJson>(api);
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
        public async Task<CheckNetworkResponseJson> CheckNetworkAsync(CheckNetworkRequestJson data, CancellationToken cancellationToken = default)
        {
            var api = "https://api.weixin.qq.com/cgi-bin/callback/check?access_token=ACCESS_TOKEN";
            var access_token = await _tokenProvider.GetTokenAsync(cancellationToken);
            api = api.Replace("ACCESS_TOKEN", access_token);
            //var data = new
            //{
            //    action = "all",
            //    check_operator = "DEFAULT"
            //};
            var result = await PostAsJsonAsync<CheckNetworkRequestJson, CheckNetworkResponseJson>(api, data);
            if (result.Succeeded)
                return result;
            else
                throw new WeixinException(result);
        }

        public Task<CheckNetworkResponseJson> CheckNetworkAsync(string action, string check_operator, CancellationToken cancellationToken = default)
            => CheckNetworkAsync(new CheckNetworkRequestJson(action, check_operator), cancellationToken);
    }
}
