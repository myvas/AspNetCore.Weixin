using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.DbContexts;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Hosts;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;
using Myvas.AspNetCore.Weixin.Models;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
/// </summary>
public static class WeixinEntityFrameworCoreBuilderExtensions
{
    /// <summary>
    /// Configures EF implementation of IPersistedGrantStore with IdentityServer.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IWeixinBuilder AddOperationalStore(
        this IWeixinBuilder builder,
        Action<WeixinStoreOptions> storeOptionsAction = null)
    {
        return builder.AddOperationalStore<WeixinDbContext>(storeOptionsAction);
    }

    /// <summary>
    /// Configures EF implementation of IPersistedGrantStore with IdentityServer.
    /// </summary>
    /// <typeparam name="TContext">The IPersistedGrantDbContext to use.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IWeixinBuilder AddOperationalStore<TContext>(
        this IWeixinBuilder builder,
        Action<WeixinStoreOptions> storeOptionsAction = null)
        where TContext : DbContext, IWeixinDbContext
    {
        builder.Services.AddOperationalDbContext<TContext>(storeOptionsAction);

        //builder.AddSigningKeyStore<SigningKeyStore>();
        builder.AddSubscribeEventReceivedEntryStore<SubscribeEventReceivedEntryStore>();

        builder.Services.AddSingleton<IHostedService, TokenCleanupHost>();

        return builder;
    }


    /// <summary>
    /// Adds an implementation of the IOperationalStoreNotification to IdentityServer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IWeixinBuilder AddOperationalStoreNotification<TNotification>(
        this IWeixinBuilder builder)
        where TNotification : class, ISubscriptionNotification
    {
        builder.Services.AddOperationalStoreNotification<TNotification>();
        return builder;
    }

    public static IWeixinBuilder AddEntityFrameworkCores<TWeixinDbContext>(this IWeixinBuilder builder)
    {
        //AddStore(builder.Services, builder.SubscriberType, builder.ReceivedTextMessageType, typeof(TWeixinDbContext));
        return builder;
    }

    #region privates
    private static void AddStore(IServiceCollection services, Type subscriberType, Type receivedTextMessageType, Type contextType)
    {
        var foundSubscriberType = FindGenericBaseType(subscriberType, typeof(WeixinSubscriber));
        if (foundSubscriberType == null)
        {
            throw new InvalidOperationException($"{nameof(subscriberType)} is not a WeixinSubscriber type");
        }
        var keyType = foundSubscriberType.GenericTypeArguments[0];//ignore it!

        if (receivedTextMessageType == null)
        {
            // no XMessageType
            Type dbContextType = null;
            var weixinDbContext = FindGenericBaseType(contextType, typeof(WeixinDbContext<>));
            if (weixinDbContext == null)
            {
                dbContextType = typeof(SubscriberOnlyStore<,>).MakeGenericType(subscriberType, contextType);
            }
            else
            {
                dbContextType = typeof(SubscriberOnlyStore<,>).MakeGenericType(subscriberType, contextType,
                    weixinDbContext.GenericTypeArguments[1],
                    weixinDbContext.GenericTypeArguments[2]);
            }
            services.TryAddScoped(typeof(IWeixinSubscriberStore<>).MakeGenericType(subscriberType), dbContextType);
        }
        //else
        //{
        //	var foundMessageType = FindGenericBaseType(receivedTextMessageType, typeof(TextMessageReceivedEntity));
        //	if (foundMessageType == null)
        //	{
        //		throw new InvalidOperationException($"{nameof(receivedTextMessageType)} is not a ReceivedTextMessageType");
        //	}

        //	Type dbContextType = null;
        //	var weixinDbContext = FindGenericBaseType(contextType, typeof(WeixinDbContext<,>));
        //	if (weixinDbContext == null)
        //	{
        //		dbContextType = typeof(SubscriberOnlyStore<,>).MakeGenericType(foundSubscriberType, foundMessageType, contextType);
        //	}
        //	else
        //	{
        //		dbContextType = typeof(SubscriberOnlyStore<,>).MakeGenericType(subscriberType, foundMessageType, contextType,
        //			weixinDbContext.GenericTypeArguments[1],
        //			weixinDbContext.GenericTypeArguments[2],
        //			weixinDbContext.GenericTypeArguments[3]);
        //	}
        //	services.TryAddScoped(typeof(IWeixinSubscriberStore<>).MakeGenericType(subscriberType, receivedTextMessageType), dbContextType);
        //}
    }

    private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
    {
        var type = currentType;
        while (type != null)
        {
            var typeInfo = type.GetTypeInfo();
            var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
            if (genericType != null && genericType == genericBaseType)
            {
                return typeInfo;
            }
            type = type.BaseType;
        }
        return null;
    }
    #endregion
}
