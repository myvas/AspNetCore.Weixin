using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin;
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
	public static WeixinBuilder AddWeixin(this IServiceCollection services, Action<WeixinOptions> setupAction = null)
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

		//Here assert IOptions<WeixinApiOptions> had already injected!
		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<WeixinOptions>, WeixinPostConfigureOptions<WeixinOptions>>());
		builder.AddAccessTokenMemoryCacheProvider();
		builder.AddWeixinAccessTokenApi();

		builder.AddBusinessApis();

		return builder;
	}
	
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

		//Here assert IOptions<WeixinApiOptions> had already injected!
		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<WeixinOptions>, WeixinPostConfigureOptions<WeixinOptions>>());
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
	public static WeixinBuilder AddWeixinJssdk(this IServiceCollection services, Action<WeixinOptions> setupAction = null)
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

		//Here assert IOptions<WeixinApiOptions> had already injected!
		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<WeixinOptions>, WeixinPostConfigureOptions<WeixinOptions>>());
		builder.AddAccessTokenMemoryCacheProvider();
		builder.AddWeixinAccessTokenApi();

		builder.AddBusinessApis();

		builder.AddJsapiTicketMemoryCacheProvider();
		builder.AddWeixinJsapiTicketApi();

		return builder;
	}
}
