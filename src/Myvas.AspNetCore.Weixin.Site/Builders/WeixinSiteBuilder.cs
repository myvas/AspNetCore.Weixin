using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Myvas.AspNetCore.Weixin
{
	/// <summary>
	/// Helper functions for configuring Weixin services.
	/// </summary>
	public class WeixinSiteBuilder
	{
		public WeixinSiteBuilder(IServiceCollection services)
		{
			Services = services;
		}

		public IServiceCollection Services { get; private set; }

		public IServiceCollection AddMessageProtectProvider()
        {
			return Services;
        }
	}
}
