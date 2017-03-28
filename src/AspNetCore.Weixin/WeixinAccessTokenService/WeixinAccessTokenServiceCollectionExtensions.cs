using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class WeixinAccessTokenServiceCollectionExtensions
    {
        /// <summary>
        /// Adds weixin access token services to the specified <see cref="IServiceCollection" />. 
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinAccessTokenOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddWeixinAccessToken(this IServiceCollection services, Action<WeixinAccessTokenOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            services.TryAddSingleton<IWeixinAccessToken, WeixinAccessTokenManager>();
            return services;
        }

        /// <summary>
        /// Adds weixin access token services to the specified <see cref="IServiceCollection" />. 
        /// <see cref="IOptions<T>"/> (where T is <see cref="WeixinAccessTokenOptions"/>) should be configured before.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWeixinAccessToken(this IServiceCollection services)
        {
            return services.AddWeixinAccessToken(setupAction:null);
        }
    }
}
