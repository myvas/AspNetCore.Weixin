using AspNetCore.Weixin.DataProtection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
	/// <summary>
	/// Extension methods for setting up Weixin message protection services in an <see cref="IServiceCollection" />.
	/// </summary>
	public static class WeixinMessageProtectionServiceCollectionExtensions
	{
		public static IServiceCollection AddWeixinMessageProtection(this IServiceCollection services)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddOptions();
			AddWeixinMessageProtectionServices(services);

			return services;
		}

		private static void AddWeixinMessageProtectionServices(IServiceCollection services)
		{
			services.TryAddEnumerable(
				ServiceDescriptor.Transient<IConfigureOptions<WeixinMessageProtectionOptions>, WeixinMessageProtectionOptionsSetup>());

			services.TryAddSingleton<IWeixinMessageEncryptor>(s =>
			{
				var dpOptions = s.GetRequiredService<IOptions<WeixinMessageProtectionOptions>>();
				var loggerFactory = s.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;

				IWeixinMessageEncryptor encryptor = new WeixinMessageEncryptor(dpOptions, loggerFactory);

				return encryptor;
			});
		}
	}
}
