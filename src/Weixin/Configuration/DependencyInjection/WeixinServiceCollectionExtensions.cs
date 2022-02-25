using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.Services;
using Myvas.AspNetCore.Weixin.Services.Default;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
/// </summary>
public static class WeixinServiceCollectionExtensions
{
    public static IWeixinBuilder AddWeixin(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var builder = new WeixinBuilder(services);

        builder.AddRequiredPlatformServices();
        builder.AddPluggableServices();

        return builder;
    }

    /// <summary>
    /// Adds weixin access token services to the specified <see cref="IServiceCollection" />. 
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinAccessTokenOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IWeixinBuilder AddWeixin(this IServiceCollection services, Action<WeixinOptions> setupAction)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (setupAction != null)
        {
            services.Configure(setupAction);
        }

        var builder = AddWeixin(services);
        //services.AddSingleton<IWeixinHandlerFactory, WeixinHandlerFactory>();
        //services.AddSingleton<IResponseBuilderFactory, ResponseBuilderFactory>();
        //services.TryAddScoped<IWeixinMessageEncryptor, WeixinMessageEncryptor>(); //即使不启用加密，也把此不必要的加密服务接口提供出来了。
        //services.AddWeixinMessageProtection();
        //services.TryAddTransient<IWeixinEventSink, TWeixinEventSink>();
        //services.AddSingleton<WeixinSite>();

        return builder;
    }

    #region Core
    /// <summary>
    /// Adds the required platform services.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IWeixinBuilder AddRequiredPlatformServices(this IWeixinBuilder builder)
    {
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddOptions();
        builder.Services.AddSingleton(
            resolver => resolver.GetRequiredService<IOptions<WeixinOptions>>().Value);
        //builder.Services.AddTransient(
        //    resolver => resolver.GetRequiredService<IOptions<WeixinSiteOptions>>().Value.MessageEncodingOptions);
        builder.Services.AddHttpClient();

        return builder;
    }

    public static IWeixinBuilder AddPluggableServices(this IWeixinBuilder builder)
    {
        builder.Services.TryAddTransient<ICancellationTokenProvider, DefaultHttpContextCancellationTokenProvider>();

        return builder;
    }
    #endregion
}

