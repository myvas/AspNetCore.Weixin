using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.Services;
using Myvas.AspNetCore.Weixin.Services.Default;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up weixin apis in an <see cref="IServiceCollection" />.
/// </summary>
public static class WeixinServiceCollectionExtensions
{
    /// <summary>
    /// Adds weixin apis to the specified <see cref="IServiceCollection" />. 
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IWeixinBuilder AddWeixin(this IServiceCollection services,
        Action<WeixinOptions> setupAction = null)
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

        builder.Services.AddOptions();
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddHttpClient();

        builder.Services.TryAddTransient<ICancellationTokenProvider, DefaultHttpContextCancellationTokenProvider>();

        // 找出本Assembly中所有继承于ApiClient的类，并注入
        builder.Services.AddHttpClient<AccessTokenApi>();
        builder.Services.AddHttpClient<JsapiTicketApi>();
        builder.Services.AddHttpClient<CardApiTicketApi>();
        builder.Services.AddHttpClient<CustomerSupportApi>();
        builder.Services.AddHttpClient<GroupMessageApi>();
        builder.Services.AddHttpClient<MediaApi>();
        builder.Services.AddHttpClient<MenuApi>();
        builder.Services.AddHttpClient<QrCodeApi>();
        builder.Services.AddHttpClient<UserApi>();
        builder.Services.AddHttpClient<UserProfileApi>();
        builder.Services.AddHttpClient<GroupsApi>();
        builder.Services.AddHttpClient<WifiApi>();

        return builder;
    }
}

