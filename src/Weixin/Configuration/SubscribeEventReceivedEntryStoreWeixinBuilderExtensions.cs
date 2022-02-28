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
public static class SubscribeEventReceivedEntryStoreWeixinBuilderExtensions
{
    /// <summary>
    /// Adds a received subscribe event store.
    /// </summary>
    /// <typeparam name="T">The type of the store that is registered in DI.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns>The builder.</returns>
    public static IWeixinBuilder AddSubscribeEventReceivedEntryStore<T>(this IWeixinBuilder builder)
        where T : class, ISubscribeEventReceivedEntryStore
    {
        builder.Services.AddTransient<ISubscribeEventReceivedEntryStore, T>();

        return builder;
    }
}