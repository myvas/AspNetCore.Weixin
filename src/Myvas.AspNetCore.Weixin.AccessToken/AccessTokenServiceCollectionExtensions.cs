using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class AccessTokenServiceCollectionExtensions
	{
		/// <summary>
		/// Adds weixin access token services to the specified <see cref="IServiceCollection" />. 
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
		/// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinAccessTokenOptions"/>.</param>
		/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
		public static IServiceCollection AddWeixinAccessToken(this IServiceCollection services, Action<WeixinAccessTokenOptions> setupAction)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			if (setupAction != null)
			{
				services.Configure(setupAction);
			}

			services.AddHttpClient<AccessTokenApi>();
			services.AddMemoryAccessTokenCacheProvider();
			services.AddTransient<IWeixinAccessToken, AccessTokenService>();
			return services;
		}

		public static IServiceCollection AddMemoryAccessTokenCacheProvider(this IServiceCollection services)
        {
			services.AddMemoryCache();
			services.AddSingleton<IWeixinAccessTokenCacheProvider, MemoryAccessTokenCacheProvider>();
			return services;
		}
	}
}
