using AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
	/// </summary>
	public static class WeixinWelcomePageServiceCollectionExtensions
	{
		public static IServiceCollection AddWeixinWelcomePage(this IServiceCollection services)
			=> services.AddWeixinWelcomePage(o => { });

		/// <summary>
		/// Adds the default identity system configuration for the specified User and Role types.
		/// </summary>
		public static IServiceCollection AddWeixinWelcomePage(this IServiceCollection services, Action<WeixinWelcomePageOptions> setupAction)
		{
			// Services Weixin depends on
			services.AddOptions().AddLogging();
			
			if (setupAction != null)
			{
				services.Configure(setupAction);
			}

			//services.TryAddScoped<IWeixinMessageEncryptor, WeixinMessageEncryptor>(); //即使不启用加密，也把此不必要的加密服务接口提供出来了。
			services.AddWeixinMessageProtection();
			
			return services;
		}
    }
}
