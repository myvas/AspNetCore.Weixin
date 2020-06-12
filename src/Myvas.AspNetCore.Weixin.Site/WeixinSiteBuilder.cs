using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
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

        public OptionsBuilder<WeixinAccessTokenOptions> AccessToken { get; set; }
        public OptionsBuilder<WeixinSiteEncoderOptions> SiteEncoder { get; set; }

        /// <summary>
        /// Gets the <see cref="Type"/> used for Weixin subscriber.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> used for Weixin subscriber.
        /// </value>
        public Type ExternalUserType { get; private set; }

        /// <summary>
        /// Gets the <see cref="Type"/> used for Weixin subscriber.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> used for Weixin subscriber.
        /// </value>
        public Type ExternalUserIdType { get; private set; }
    }
}
