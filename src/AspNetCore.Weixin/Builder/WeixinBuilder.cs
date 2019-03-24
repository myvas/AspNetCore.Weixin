using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace AspNetCore.Weixin
{
	/// <summary>
	/// Helper functions for configuring Weixin services.
	/// </summary>
	public class WeixinBuilder
	{
		public IServiceCollection Services { get; private set; }

		public WeixinBuilder(IServiceCollection services)
		{
			Services = services;
		}

		public virtual WeixinBuilder AddWelcomePage(Action<WeixinWelcomePageOptions> setupAction)
		{
			if (setupAction != null)
			{
				Services.Configure(setupAction);
			}

			Services.TryAddScoped<IWeixinMessageEncryptor, WeixinMessageEncryptor>();

			return this;
		}
	}
}
