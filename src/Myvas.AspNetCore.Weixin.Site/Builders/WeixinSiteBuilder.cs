﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Myvas.AspNetCore.Weixin
{
	/// <summary>
	/// Helper functions for configuring Weixin services.
	/// </summary>
	public class WeixinSiteBuilder
	{
		public WeixinSiteBuilder(Type subscriberType, IServiceCollection services)
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