using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http.Json;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Interfaces;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up weixin access token services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class AccessTokenBuilderExtensions
    {
        /// <summary>
        /// Adds weixin access token services to the specified <see cref="IServiceCollection" />. 
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An action delegate to configure the provided <see cref="WeixinAccessTokenOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IWeixinBuilder AddAccessToken(
            this IWeixinBuilder builder,
            Action<RedisCacheOptions> storeOptionsAction = null,
            Action<WeixinAccessTokenOptions> setupAction = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddStackExchangeRedisCache(storeOptionsAction);

            builder.Services.AddHttpClient<AccessTokenApi>();
            //builder.Services.AddMemoryAccessTokenCacheProvider();
            builder.Services.AddTransient<IWeixinAccessToken, AccessTokenService>();

            return builder;
        }

        [Obsolete("Use Microsoft.Extensions.Caching.StackExchangeRedis instead.")]
        private static IServiceCollection AddMemoryAccessTokenCacheProvider(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IWeixinAccessTokenCacheProvider, MemoryAccessTokenCacheProvider>();
            return services;
        }
    }
}
