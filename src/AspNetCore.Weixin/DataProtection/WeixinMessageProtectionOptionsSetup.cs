using AspNetCore.Weixin.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
	internal class WeixinMessageProtectionOptionsSetup : IConfigureOptions<WeixinMessageProtectionOptions>
	{
		private readonly ILoggerFactory _loggerFactory;

		public WeixinMessageProtectionOptionsSetup() : this(NullLoggerFactory.Instance)
		{
		}

		public WeixinMessageProtectionOptionsSetup(ILoggerFactory loggerFactory)
		{
			_loggerFactory = loggerFactory;
		}

		public void Configure(WeixinMessageProtectionOptions options)
		{
			if (string.IsNullOrWhiteSpace(options.AppId))
			{
				throw new ArgumentNullException(nameof(options.AppId));
			}

			if (string.IsNullOrWhiteSpace(options.WebsiteToken))
			{
				throw new ArgumentNullException(nameof(options.WebsiteToken));
			}

			if (string.IsNullOrWhiteSpace(options.EncodingAesKey))
			{
				throw new ArgumentNullException(nameof(options.EncodingAesKey));
			}
		}
	}
}