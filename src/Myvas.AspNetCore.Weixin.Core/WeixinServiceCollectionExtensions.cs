using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http.Json;
using Myvas.AspNetCore.Weixin.Site.ResponseBuilder;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class WeixinServiceCollectionExtensions
    {
        public static WeixinBuilder AddWeixin(this IServiceCollection services, Action<WeixinOptions> setupAction)
        {
            return AddWeixinSite<WeixinSubscriber>(services, setupAction);
        }

        /// <summary>
        /// Adds weixin access token services to the specified <see cref="IServiceCollection" />. 
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinAccessTokenOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static WeixinBuilder AddWeixinSite<TWeixinSubscriber>(this IServiceCollection services, Action<WeixinSiteOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            services.AddSingleton<IWeixinHandlerFactory, WeixinHandlerFactory>();
            services.AddSingleton<IResponseBuilderFactory, ResponseBuilderFactory>();
            //services.TryAddScoped<IWeixinMessageEncryptor, WeixinMessageEncryptor>(); //即使不启用加密，也把此不必要的加密服务接口提供出来了。
            //services.AddWeixinMessageProtection();
            //services.TryAddTransient<IWeixinEventSink, TWeixinEventSink>();
            services.AddSingleton<WeixinSite>();

            return new WeixinBuilder(typeof(TWeixinSubscriber), services);
        }
    }
}
