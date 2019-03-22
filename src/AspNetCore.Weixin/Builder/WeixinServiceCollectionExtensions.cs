using AspNetCore.Weixin;
using AspNetCore.Weixin.DataProtection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Contains extension methods to <see cref="IServiceCollection"/> for configuring Weixin services.
	/// </summary>
	public static class WeixinServiceCollectionExtensions
	{
		public static WeixinBuilder AddWeixin(this IServiceCollection services)
			=> services.AddWeixin(o => { });

		public static WeixinBuilder AddWeixin(this IServiceCollection services, Action<WeixinAccessTokenOptions> setupAction)
		{
			// Services Weixin depends on
			services.AddOptions().AddLogging();

			// Services used by Weixin
			services.TryAddScoped<IWeixinAccessToken, MemoryCachedWeixinAccessToken>();

			if (setupAction != null)
			{
				services.Configure(setupAction);
			}

			return new WeixinBuilder(services);
		}
	}
}
