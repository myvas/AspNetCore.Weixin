using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up Weixin message protection services in an <see cref="IServiceCollection" />.
/// </summary>
public static class EncodingServiceCollectionExtensions
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
		services.TryAddSingleton<IWeixinMessageEncryptor>(s =>
		{
			var wxoptionsAccessor = s.GetRequiredService<IOptions<WeixinOptions>>();
			var optionsAccessor = s.GetRequiredService<IOptions<WeixinSiteOptions>>();
			var loggerFactory = s.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;

			IWeixinMessageEncryptor encryptor = new WeixinMessageEncryptor(wxoptionsAccessor, optionsAccessor, loggerFactory);

			return encryptor;
		});
	}
}
