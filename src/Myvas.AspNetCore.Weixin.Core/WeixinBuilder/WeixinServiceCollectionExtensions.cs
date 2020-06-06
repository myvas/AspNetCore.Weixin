using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Contains extension methods to <see cref="IServiceCollection"/> for configuring Weixin services.
	/// </summary>
	public static class WeixinServiceCollectionExtensions
	{
		public static WeixinBuilder AddWeixin(this IServiceCollection services, Action<WeixinAccessTokenOptions> setupAction)
		{
            if (services == null)
            {
				throw new ArgumentNullException(nameof(services));
            }

			// Services Weixin depends on
			services.AddOptions();

			if (setupAction != null)
			{
				services.Configure(setupAction);
			}

			return new WeixinBuilder(services);
		}
	}
}
