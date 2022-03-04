using Microsoft.Extensions.Caching.StackExchangeRedis;
using Myvas.AspNetCore.Weixin;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
    /// </summary>
    /// <remarks>Dependent on: AddWeixin()</remarks>
    public static class WeixinAccessTokenBuilderExtensions
    {
        /// <summary>
        /// Adds weixin access token services to the specified <see cref="IServiceCollection" />. 
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IWeixinBuilder AddAccessToken(this IWeixinBuilder builder,
            Action<RedisCacheOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction != null)
            {
                builder.Services.Configure(setupAction);
            }

            //builder.Services.AddWeixin();
            builder.Services.AddStackExchangeRedisCache(setupAction);
            builder.Services.AddTransient<IWeixinAccessToken, AccessTokenService>();

            return builder;
        }
    }
}
