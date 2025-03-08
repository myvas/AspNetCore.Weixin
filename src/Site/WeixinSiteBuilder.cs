using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;

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

        private IList<IWeixinHandler> _handlers = new List<IWeixinHandler>();
        public IReadOnlyList<IWeixinHandler> WeixinHandlers { get { return (IReadOnlyList<IWeixinHandler>)_handlers; } }
        public IWeixinHandler AddHandler(IWeixinHandler handler)
        {
            if (!_handlers.Contains(handler))
            {
                _handlers.Add(handler);
            }
            return handler;
        }

        public OptionsBuilder<WeixinOptions> ApiOptions { get; set; }
        public OptionsBuilder<WeixinSiteOptions> SiteOptions { get; set; }
        public OptionsBuilder<WeixinSiteEncodingOptions> EncodingOptions { get; set; }

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
