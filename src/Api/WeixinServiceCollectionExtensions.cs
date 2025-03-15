﻿using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
/// </summary>
public static class WeixinServiceCollectionExtensions
{
	/// <summary>
	/// Adds weixin access token services to the specified <see cref="IServiceCollection" />. 
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
	/// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinOptions"/>.</param>
	/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
	public static WeixinBuilder AddWeixinCore(this IServiceCollection services, Action<WeixinOptions> setupAction = null)
	{
		if (services == null)
		{
			throw new ArgumentNullException(nameof(services));
		}

		if (setupAction != null)
		{
			services.Configure(setupAction);
		}

		var builder = new WeixinBuilder(services);
		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<WeixinOptions>, WeixinPostConfigureOptions<WeixinOptions>>());

		builder.Services.AddLogging();
		builder.AddAccessTokenMemoryCacheProvider();
		builder.AddWeixinAccessTokenApi();

		return builder;
	}

	/// <summary>
	/// Adds weixin access token services to the specified <see cref="IServiceCollection" />. 
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
	/// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinOptions"/>.</param>
	/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
	public static WeixinBuilder AddWeixin(this IServiceCollection services, Action<WeixinOptions> setupAction = null)
	{
		var builder = services.AddWeixinCore(setupAction);

		builder.AddJsapiTicketMemoryCacheProvider();
		builder.AddWeixinJsapiTicketApi();

        builder.Services.AddTransient<IWeixinCommonApi, WeixinCommonApi>();
        builder.Services.AddTransient<IWeixinMenuApi, WeixinMenuApi>();
        builder.Services.AddTransient<ICardApiTicketApi, CardApiTicketApi>();
        builder.Services.AddTransient<ICustomerSupportApi, CustomerSupportApi>();
        builder.Services.AddTransient<IGroupMessageApi, GroupMessageApi>();
        builder.Services.AddTransient<IMediaApi, MediaApi>();
        builder.Services.AddTransient<IQrcodeApi, QrcodeApi>();
        builder.Services.AddTransient<IUserApi, UserApi>();
        builder.Services.AddTransient<IGroupsApi, GroupsApi>();
        builder.Services.AddTransient<IUserProfileApi, UserProfileApi>();
        builder.Services.AddTransient<IWifiApi, WifiApi>();

		return builder;
	}
}
