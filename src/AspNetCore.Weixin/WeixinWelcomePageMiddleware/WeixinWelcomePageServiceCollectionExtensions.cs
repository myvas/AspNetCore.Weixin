using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
    /// </summary>
    public static class WeixinWelcomePageServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default identity system configuration for the specified User and Role types.
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <typeparam name="TRole">The type representing a Role in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
        public static IServiceCollection AddWeixinWelcomePage(
            this IServiceCollection services,
            Action<WeixinWelcomePageOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
            return services;
        }

        /// <summary>
        /// Adds the default identity system configuration for the specified User and Role types.
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <typeparam name="TRole">The type representing a Role in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
        public static IServiceCollection AddWeixinWelcomePage(
            this IServiceCollection services)
        {
            return services.AddWeixinWelcomePage(setupAction: null);
        }
    }
}
