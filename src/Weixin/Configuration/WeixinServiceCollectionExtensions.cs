using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.StackExchangeRedis;
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
    /// <summary>
    /// Adds weixin access token services to the specified <see cref="IServiceCollection" />. 
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IWeixinBuilder AddWeixin(this IServiceCollection services, Action<WeixinOptions> setupAction, Action<RedisCacheOptions> redisCacheAction)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (redisCacheAction == null)
        {
            throw new ArgumentNullException(nameof(redisCacheAction));
        }

        if (setupAction != null)
        {
            services.Configure(setupAction);
        }

        var builder = new WeixinBuilder(services);

        builder.Services.AddOptions();
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //builder.Services.AddSingleton(
        //    resolver => resolver.GetRequiredService<IOptions<WeixinOptions>>().Value);
        //builder.Services.AddSingleton(
        //    resolver => resolver.GetRequiredService<IOptions<RedisCacheOptions>>().Value);
        builder.Services.AddHttpClient();

        builder.Services.TryAddTransient<ICancellationTokenProvider, DefaultHttpContextCancellationTokenProvider>();

        builder.AddAccessToken(redisCacheAction);

        return builder;
    }
}

