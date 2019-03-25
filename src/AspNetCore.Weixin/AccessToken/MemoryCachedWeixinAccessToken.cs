using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Weixin
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>DI: Singleton</remarks>
	public class MemoryCachedWeixinAccessToken : IWeixinAccessToken
	{
		private static string CachePrefix = Guid.NewGuid().ToString("N");
		private string GenerateCacheKey(string appId) { return $"{CachePrefix}_{appId}"; }

		private readonly IMemoryCache _cache;
		private WeixinAccessTokenOptions _options;

		public MemoryCachedWeixinAccessToken(IMemoryCache cache, IOptions<WeixinAccessTokenOptions> optionsAccessor)
		{
			_cache = cache;
			_options = optionsAccessor.Value;
		}

		public string GetToken(bool forceRenew)
		{
			var appId = _options.AppId;
			var appSecret = _options.AppSecret;
			var cacheKey = GenerateCacheKey(appId);

			if (!forceRenew)
			{
				if (_cache.TryGetValue(cacheKey, out string accessToken))
				{
					return accessToken;
				}
			}
			else
			{
				_cache.Remove(cacheKey);
			}

			try
			{
				var json = AccessTokenApi.GetTokenAsync(appId, appSecret).Result;
				if (json == null
					|| string.IsNullOrWhiteSpace(json.access_token)
					|| json.expires_in < 1
					|| json.expires_in > 7200)
				{
					throw WeixinAccessTokenError.UnknownResponse();
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
				throw WeixinAccessTokenError.GenericError(ex);
			}
		}

		public string GetToken()
		{
			return GetToken(false);
		}
	}
}
