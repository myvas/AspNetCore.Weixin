using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Myvas.AspNetCore.Weixin
{
    public static class WeixinSiteBuilderExtensions
    {
		public static WeixinSiteBuilder AddWeixinSite(this WeixinBuilder builder)
        {
        }

		public static WeixinSiteBuilder AddStores<TUserKey>(this WeixinBuilder builder)
		{
		}
	}

	/// <summary>
	/// Helper functions for configuring Weixin services.
	/// </summary>
	public class WeixinBuilder
	{
		public WeixinBuilder(Type subscriberType, IServiceCollection services)
		{
			SubscriberType = subscriberType;
			Services = services;
		}

		public IServiceCollection Services { get; private set; }

        /// <summary>
        /// Gets the <see cref="Type"/> used for Weixin subscriber.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> used for Weixin subscriber.
        /// </value>
        public Type SubscriberType { get; private set; }
    }
}
