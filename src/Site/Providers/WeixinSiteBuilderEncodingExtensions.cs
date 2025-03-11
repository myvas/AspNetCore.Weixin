using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up Weixin message protection services in the <see cref="WeixinSiteBuilder.Services" />.
/// </summary>
public static class WeixinSiteBuilderEncodingExtensions
{
	public static WeixinSiteBuilder AddWeixinMessageEncryptor(this WeixinSiteBuilder builder, Action<WeixinSiteEncodingOptions> setupAction = null)
	{
		if (builder == null) throw new ArgumentNullException(nameof(builder));

		if (setupAction != null) builder.Services.Configure(setupAction);
		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<WeixinSiteEncodingOptions>, WeixinSiteEncodingPostConfigureOptions<WeixinSiteEncodingOptions>>());

		// It is safe to call this method multiple times and from different places.
		// We call it here just make sure ILoggerFactory/ILogger/Logger<T> is added to the builder.Services.
		builder.Services.AddLogging();
		builder.Services.TryAddSingleton<IWeixinMessageEncryptor, WeixinMessageEncryptor>();
		return builder;
	}
}
