using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http.Json;
using Myvas.AspNetCore.Weixin.Site.ResponseBuilder;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Myvas.AspNetCore.Weixin.Services;
using Myvas.AspNetCore.Weixin.Services.Default;
using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.Models;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up <see cref="WeixinSiteMiddleware"/> in an <see cref="IWeixinBuilder" />.
/// </summary>
/// <remarks>Dependent on: AddWeixin().AddAccessToken();</remarks>
public static class WeixinSiteBuilderExtensions
{
    /// <summary>
    /// Adds <see cref="WeixinSiteMiddleware"/> to the specified <see cref="IWeixinBuilder" />. 
    /// </summary>
    /// <typeparam name="TWeixinEventSink">The <see cref="IWeixinEventSink"/> and <see cref="WeixinEventSink"/>.</typeparam>
    /// <typeparam name="TSubscriber">The <see cref="Subscriber"/>.</typeparam>
    /// <typeparam name="TContext">The <see cref="IWeixinDbContext"/> and <see cref="WeixinDbContext"/>.</typeparam>
    /// <param name="builder">The <see cref="IWeixinBuilder" /> to add services to.</param>
    /// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinSiteOptions"/>.</param>
    /// <returns>The <see cref="IWeixinBuilder"/> so that additional calls can be chained.</returns>
    public static IWeixinBuilder AddWeixinSite<TWeixinEventSink, TSubscriber, TContext>(this IWeixinBuilder builder, Action<WeixinSiteOptions> setupAction)
        where TWeixinEventSink : class, IWeixinEventSink
        where TSubscriber : Subscriber, new()
        where TContext : DbContext, IWeixinDbContext<TSubscriber>
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (setupAction != null)
        {
            builder.Services.Configure(setupAction);
        }

        builder.AddEntityFrameworkStores<TSubscriber, TContext>();
        builder.AddSubscriberManager<TSubscriber, TContext>();
        builder.Services.AddSingleton<IWeixinHandlerFactory, WeixinHandlerFactory>();
        //builder.Services.TryAddScoped<IWeixinMessageEncryptor, WeixinMessageEncryptor>(); //即使不启用加密，也把此不必要的加密服务接口提供出来了。
        //builder.Services.AddWeixinMessageProtection();
        builder.Services.AddWeixinResponseBuilder();
        builder.Services.TryAddScoped<IWeixinEventSink, TWeixinEventSink>();
        builder.Services.AddSingleton<WeixinSite>();

        return builder;
    }
}

