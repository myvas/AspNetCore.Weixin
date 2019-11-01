using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
	/// <summary>
	/// jsapi_ticket是公众号用于调用微信JS接口的临时票据。
	/// <para>正常情况下，jsapi_ticket的有效期为7200秒，通过access_token来获取。</para>
	/// <para>由于获取jsapi_ticket的api调用次数非常有限，频繁刷新jsapi_ticket会导致api调用受限，影响自身业务，开发者必须在自己的服务全局缓存jsapi_ticket。</para>
	/// <para>支持多AccessToken</para>
	/// </summary>
	public class MemoryCachedWeixinJsapiTicket : IWeixinJsapiTicket
	{
		private static string CachePrefix = Guid.NewGuid().ToString("N");
		private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

		private readonly IMemoryCache _cache;
		private readonly WeixinJssdkOptions _options;
		private readonly IWeixinAccessToken _AccessToken;

		public MemoryCachedWeixinJsapiTicket(IMemoryCache cache, WeixinJssdkOptions options, IWeixinAccessToken AccessToken)
		{
			_cache = cache;
			_options = options;
			_AccessToken = AccessToken;
		}

		/// <summary>
		/// 获取微信JSSDK访问凭证。仅在需要时调用微信JSSDK接口，即：若凭证尚在有效期内，则直接取回上一次得到的凭证。
		/// </summary>
		/// <param name="accessToken"></param>
		/// <param name="forceRenew">强制立即取新。这将废弃并替换旧的，而不管是否旧的令牌是否过期。</param>
		/// <returns>微信JSSDK访问凭证</returns>
		private string GetTicket(string accessToken, bool forceRenew)
		{
			var appId = _options.AppId;
			var cacheKey = GenerateCacheKey(appId);

			if (!forceRenew)
			{
				if (_cache.TryGetValue(cacheKey, out string jsapiTicket))
				{
					return jsapiTicket;
				}
			}
			else
			{
				_cache.Remove(cacheKey);
			}

			try
			{
				var json = JsapiTicketApi.GetJsapiTicket(accessToken).Result;
				if (json == null
				   || string.IsNullOrWhiteSpace(json.ticket)
				   || json.expires_in < 1
				   || json.expires_in > 7200)
				{
					throw WeixinJsapiTicketError.UnknownResponse();
				}
				else
				{
					var newJsapiTicket = json.ticket;
					_cache.Set(cacheKey, newJsapiTicket, TimeSpan.FromSeconds(json.expires_in));
					return newJsapiTicket;
				}
			}
			catch (Exception ex)
			{
				throw WeixinJsapiTicketError.GenericError(ex);
			}
		}

		private string GetTicket(string accessToken)
		{
			return GetTicket(accessToken, false);
		}

		public string GetTicket(bool forceRenew)
		{
			var accessToken = _AccessToken.GetToken();
			return GetTicket(accessToken, forceRenew);
		}

		public string GetTicket()
		{
			var accessToken = _AccessToken.GetToken();
			return GetTicket(accessToken);
		}

	}
}
