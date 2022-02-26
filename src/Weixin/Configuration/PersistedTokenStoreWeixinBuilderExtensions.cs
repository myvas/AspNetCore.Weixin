using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Builder extension methods for registering additional services 
/// </summary>
public static class PersistedTokenStoreWeixinBuilderExtensions
{
    /// <summary>
    /// Adds a persisted token store.
    /// </summary>
    /// <typeparam name="T">The type of the concrete token store that is registered in DI.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns>The builder.</returns>
    public static IWeixinBuilder AddPersistedTokenStore<T>(this IWeixinBuilder builder)
        where T : class, IPersistedTokenStore
    {
        builder.Services.AddTransient<IPersistedTokenStore, T>();

        return builder;
    }
}