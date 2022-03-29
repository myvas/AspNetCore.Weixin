using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up weixin jsapi ticket services in an <see cref="IWeixinBuilder" />.
/// </summary>
/// <remarks>Dependent on: AddWeixin().AddAccessToken()</remarks>
public static class WeixinJssdkBuilderExtensions
{
    /// <summary>
    /// Adds jssdk services to the specified <see cref="IWeixinBuilder" />. 
    /// </summary>
    /// <param name="builder">The <see cref="IWeixinBuilder" /> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IWeixinBuilder AddJssdk(this IWeixinBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.TryAddSingleton(o =>
        {
            return new WeixinJssdkOptions(o.GetService<WeixinOptions>());
        });
        //builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<WeixinJssdkOptions>, WeixinJssdkPostConfigureOptions>());
        builder.Services.TryAddTransient<IWeixinJsapiTicket, WeixinJsapiTicketService>();

        return builder;
    }
}
