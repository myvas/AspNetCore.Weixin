using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// jsapi_ticket是公众号用于调用微信JS接口的临时票据。
    /// <para>正常情况下，jsapi_ticket的有效期为7200秒，通过access_token来获取。</para>
    /// <para>由于获取jsapi_ticket的api调用次数非常有限，频繁刷新jsapi_ticket会导致api调用受限，影响自身业务，开发者必须在自己的服务全局缓存jsapi_ticket。</para>
    /// <para>支持多AccessToken</para>
    /// </summary>
    public class WeixinJsapiTicketService : IWeixinJsapiTicket
    {
        private string GenerateCacheKey(string appId) { return $"jsapi_{appId}"; }

        private readonly IDistributedCache _cache;
        private readonly WeixinJssdkOptions _options;
        private readonly JsapiTicketApi _api;
        private readonly IWeixinAccessToken _token;

        public WeixinJsapiTicketService(IDistributedCache cache,
            IOptions<WeixinJssdkOptions> optionsAccessor,
            IWeixinAccessToken token,
            JsapiTicketApi api)
        {
            _cache = cache;
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        /// <summary>
        /// 获取微信JSSDK访问凭证。仅在需要时调用微信JSSDK接口，即：若凭证尚在有效期内，则直接取回上一次得到的凭证。
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="forceRenew">强制立即取新。这将废弃并替换旧的，而不管是否旧的令牌是否过期。</param>
        /// <returns>微信JSSDK访问凭证</returns>
        private async Task<string> GetTicketAsync(string accessToken, bool forceRenew)
        {
            var appId = _options.AppId;
            var cacheKey = GenerateCacheKey(appId);

            if (forceRenew)
            {
                _cache.Remove(cacheKey);
                accessToken = _token.GetToken(true);
            }

            try
            {
                var ticket = await _cache.GetStringAsync(cacheKey);
                if (string.IsNullOrWhiteSpace(ticket))
                {
                    var json = await _api.GetJsapiTicket(accessToken);
                    if (json == null
                       || string.IsNullOrWhiteSpace(json.ticket)
                       || json.expires_in < 1
                       || json.expires_in > 7200)
                    {
                        throw WeixinJsapiTicketError.UnknownResponse();
                    }
                    else
                    {
                        ticket = json.ticket;
                        var cacheEntryOptions = new DistributedCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(json.expires_in));
                        await _cache.SetStringAsync(cacheKey, ticket, cacheEntryOptions);
                    }
                }
                return ticket;
            }
            catch (Exception ex)
            {
                throw WeixinJsapiTicketError.GenericError(ex);
            }
        }

        /// <summary>
        /// 获取微信JSSDK访问凭证。仅在需要时调用微信JSSDK接口，即：若凭证尚在有效期内，则直接取回上一次得到的凭证。
        /// </summary>
        /// <param name="forceRenew">强制立即取新。这将废弃并替换旧的，而不管是否旧的令牌是否过期。</param>
        /// <returns>微信JSSDK访问凭证</returns>
        public async Task<string> GetTicketAsync(bool forceRenew)
        {
            var accessToken = _token.GetToken();
            return await GetTicketAsync(accessToken, forceRenew);
        }

        /// <summary>
        /// 获取微信JSSDK访问凭证。仅在需要时调用微信JSSDK接口，即：若凭证尚在有效期内，则直接取回上一次得到的凭证。
        /// </summary>
        /// <param name="forceRenew">强制立即取新。这将废弃并替换旧的，而不管是否旧的令牌是否过期。</param>
        /// <returns>微信JSSDK访问凭证</returns>
        public string GetTicket(bool forceRenew) => GetTicketAsync(forceRenew).Result;
    }
}
