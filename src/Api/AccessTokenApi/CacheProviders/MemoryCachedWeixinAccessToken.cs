using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Services
{
	/// <summary>
	/// jsapi_ticket是公众号用于调用微信JS接口的临时票据。
	/// <para>正常情况下，jsapi_ticket的有效期为7200秒，通过access_token来获取。</para>
	/// <para>由于获取jsapi_ticket的api调用次数非常有限，频繁刷新jsapi_ticket会导致api调用受限，影响自身业务，开发者必须在自己的服务全局缓存jsapi_ticket。</para>
	/// <para>支持多AccessToken</para>
	/// </summary>
	public class MemoryCachedWeixinAccessToken : IWeixinAccessToken
	{
		private static string CachePrefix = Guid.NewGuid().ToString("N");
		private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

		private readonly IMemoryCache _cache;
		private readonly WeixinApiOptions _options;
		private readonly WeixinAccessTokenApi _api;

		public MemoryCachedWeixinAccessToken(IMemoryCache cache,
			IOptions<WeixinApiOptions> optionsAccessor,
			WeixinAccessTokenApi api)
		{
			_cache = cache?? throw new ArgumentNullException(nameof(cache));
			_options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
			_api = api ?? throw new ArgumentNullException(nameof(api));
		}

		/// <summary>
		/// 获取微信JSSDK访问凭证。仅在需要时调用微信JSSDK接口，即：若凭证尚在有效期内，则直接取回上一次得到的凭证。
		/// </summary>
		/// <param name="accessToken"></param>
		/// <param name="forceRenew">强制立即取新。这将废弃并替换旧的，而不管是否旧的令牌是否过期。</param>
		/// <returns>微信JSSDK访问凭证</returns>
		public async Task<string> GetTokenAsync(bool forceRenew, CancellationToken cancellationToken = default)
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
				var json = await _api.GetTokenAsync();
				if (json == null
				   || string.IsNullOrWhiteSpace(json.access_token)
				   || json.expires_in < 1
				   || json.expires_in > 7200)
				{
					throw new WeixinException(json);
				}
				else
				{
					var newAccessToken = json.access_token;
					_cache.Set(cacheKey, newAccessToken, TimeSpan.FromSeconds(json.expires_in));
					return newAccessToken;
				}
			}
			catch (Exception ex)
			{
				throw new WeixinException(ex.Message, ex.InnerException);
			}
		}

		public Task<string> GetTokenAsync(CancellationToken cancellationToken = default) => GetTokenAsync(false, cancellationToken);
		public string GetToken() => Task.Run(async () => await GetTokenAsync()).Result;
		public string GetToken(bool forceRenew) => Task.Run(async () => await GetTokenAsync(forceRenew)).Result;
	}
}