using AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
	/// </summary>
	public static class WeixinJssdkServiceCollectionExtensions
	{
		/// <summary>
		/// Adds weixin jssdk services to the specified <see cref="IServiceCollection" />. 
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
		/// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinJssdkOptions"/>.</param>
		/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
		public static IServiceCollection AddWeixinJssdk(this IServiceCollection services, Action<WeixinJssdkOptions> setupAction)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			if (setupAction != null)
			{
				services.Configure(setupAction);
			}

			services.AddMemoryCache();
			services.TryAddSingleton<IWeixinJsapiTicket, MemoryCachedWeixinJsapiTicket>();
			return services;
		}

		/// <summary>
		/// Adds weixin jssdk services to the specified <see cref="IServiceCollection" />. 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddWeixinJssdk(this IServiceCollection services)
		{
			return services.AddWeixinJssdk(setupAction: null);
		}
	}
}
