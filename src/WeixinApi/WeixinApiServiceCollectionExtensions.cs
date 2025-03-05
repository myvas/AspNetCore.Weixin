using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http.Json;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
	/// </summary>
	public static class WeixinApiServiceCollectionExtensions
	{
		/// <summary>
		/// Adds weixin access token services to the specified <see cref="IServiceCollection" />. 
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
		/// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinApiOptions"/>.</param>
		/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
		public static WeixinApiBuilder AddWeixinApi(this IServiceCollection services, Action<WeixinApiOptions> setupAction = null)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			if (setupAction != null)
			{
				services.Configure(setupAction);
			}

			var builder = new WeixinApiBuilder(services);
			builder.AddWeixinAccessTokenServices();
			builder.AddWeixinApiServices();
			builder.AddDefaultAccessTokenCacheProvider();
			return builder;
		}
	}
}
