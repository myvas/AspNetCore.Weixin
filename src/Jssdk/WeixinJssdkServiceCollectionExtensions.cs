using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class WeixinJssdkServiceCollectionExtensions
    {
        /// <summary>
        /// Adds weixin jssdk services to the specified <see cref="IServiceCollection" />. 
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinJssdkOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static WeixinBuilder AddWeixinJssdkServices(this WeixinBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            //Here assert IOptions<WeixinApiOptions> had already injected!
            //builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<WeixinApiOptions>, WeixinApiPostConfigureOptions<WeixinApiOptions>>());
            builder.Services.AddTransient<WeixinJsapiTicketDirectApi>();
            builder.Services.AddSingleton<IWeixinJsapiTicketApi, WeixinJsapiTicketApi>();
            return builder;
        }

        public static WeixinBuilder AddDefaultJsapiTicketCacheProvider(this WeixinBuilder builder)
        {
            builder.Services.AddMemoryCache();
            builder.Services.Where(x => x.ImplementationType == typeof(IWeixinJsapiTicketCacheProvider)).ToList()
                .ForEach(x => builder.Services.Remove(x));
            builder.Services.AddSingleton<IWeixinJsapiTicketCacheProvider, WeixinJsapiTicketMemoryCacheProvider>();
            return builder;
        }

        public static WeixinBuilder AddJsapiTicketCacheProvider<TCacheProvider>(this WeixinBuilder builder)
            where TCacheProvider : class, IWeixinJsapiTicketCacheProvider
        {
            builder.Services.Where(x => x.ImplementationType == typeof(IWeixinJsapiTicketCacheProvider)).ToList()
                .ForEach(x => builder.Services.Remove(x));
            builder.Services.AddSingleton<IWeixinJsapiTicketCacheProvider, TCacheProvider>();
            builder.Services.AddSingleton<TCacheProvider>();
            return builder;
        }
    }
}
