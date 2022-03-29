using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.DbContexts;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;
using Myvas.AspNetCore.Weixin.Models;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contains extension methods to <see cref="IWeixinBuilder"/> for <see cref="IWeixinDbContext{TSubscriber}"/>.
/// </summary>
public static class WeixinEntityFrameworCoreBuilderExtensions
{
    /// <summary>
    /// Configures IXxxStore implementation.
    /// </summary>
    /// <typeparam name="TSubscriber">The <see cref="Subscriber"/>.</typeparam>
    /// <typeparam name="TContext">The <see cref="DbContext"/>.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IWeixinBuilder AddEntityFrameworkStores<TSubscriber, TContext>(
        this IWeixinBuilder builder,
        Action<WeixinStoreOptions> storeOptionsAction = null)
        where TSubscriber : Subscriber
        where TContext : DbContext, IWeixinDbContext<TSubscriber>
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (storeOptionsAction != null)
        {
            builder.Services.Configure(storeOptionsAction);
        }

        builder.Services.AddWeixinDbContext<TSubscriber, TContext>(storeOptionsAction);

        builder.Services.TryAddSubscriberStore(typeof(TSubscriber), typeof(TContext));

        builder.Services.TryAddScoped<IReceivedEntryStore<EventReceivedEntry>, EventReceivedEntryStore<TContext>>();
        builder.Services.TryAddScoped<IReceivedEntryStore<MessageReceivedEntry>, MessageReceivedEntryStore<TContext>>();

        //builder.Services.AddTransient<TokenCleanupService>();

        return builder;
    }


    /// <summary>
    /// Adds an implementation of the <see cref="ISubscriptionNotification"/> to <see cref="IWeixinBuilder"/>.
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IWeixinBuilder AddSubscriptionNotification<TNotification>(this IWeixinBuilder builder)
        where TNotification : class, ISubscriptionNotification
    {
        builder.Services.AddTransient<ISubscriptionNotification, TNotification>();

        return builder;
    }


    #region privates

    /// <summary>
    /// Adds operational DbContext to the DI system.
    /// </summary>
    /// <typeparam name="TSubscriber">The <see cref="Subscriber"/>.</typeparam>
    /// <typeparam name="TContext">The <see cref="WeixinDbContext{TSubscriber}"/>.</typeparam>
    /// <param name="services"></param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    private static IServiceCollection AddWeixinDbContext<TSubscriber, TContext>(this IServiceCollection services,
        Action<WeixinStoreOptions> storeOptionsAction = null)
        where TSubscriber : Subscriber
        where TContext : DbContext, IWeixinDbContext<TSubscriber>
    {
        var storeOptions = new WeixinStoreOptions();
        services.AddSingleton(storeOptions);
        storeOptionsAction?.Invoke(storeOptions);

        if (storeOptions.ResolveDbContextOptions != null)
        {
            if (storeOptions.EnablePooling)
            {
                if (storeOptions.PoolSize.HasValue)
                {
                    services.AddDbContextPool<TContext>(storeOptions.ResolveDbContextOptions,
                        storeOptions.PoolSize.Value);
                }
                else
                {
                    services.AddDbContextPool<TContext>(storeOptions.ResolveDbContextOptions);
                }
            }
            else
            {
                services.AddDbContext<TContext>(storeOptions.ResolveDbContextOptions);
            }
        }
        else
        {
            if (storeOptions.EnablePooling)
            {
                if (storeOptions.PoolSize.HasValue)
                {
                    services.AddDbContextPool<TContext>(
                        dbCtxBuilder => { storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder); },
                        storeOptions.PoolSize.Value);
                }
                else
                {
                    services.AddDbContextPool<TContext>(
                        dbCtxBuilder => { storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder); });
                }
            }
            else
            {
                services.AddDbContext<TContext>(dbCtxBuilder =>
                {
                    storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder);
                });
            }
        }

        services.TryAddScoped<IWeixinDbContext<TSubscriber>, TContext>();
        //services.TryAddScoped<WeixinDbContext<TSubscriber>, TContext>();

        return services;
    }

    private static IServiceCollection TryAddSubscriberStore(this IServiceCollection services, Type subscriberType, Type contextType)
    {
        var foundSubscriberType = FindGenericBaseType(subscriberType, typeof(Subscriber));
        if (foundSubscriberType == null)
        {
            //throw new InvalidOperationException($"{nameof(subscriberType)} is not a Subscriber type");
            Type subscriberStoreType = null;
            var weixinDbContext = FindGenericBaseType(contextType, typeof(WeixinDbContext<>));
            if (weixinDbContext == null)
            {
                subscriberStoreType = typeof(SubscriberStore<,>).MakeGenericType(subscriberType, contextType);
            }
            else
            {
                subscriberStoreType = typeof(SubscriberStore<,>).MakeGenericType(subscriberType, contextType);
                //subscriberStoreType = typeof(SubscriberStore<,>).MakeGenericType(subscriberType, contextType,
                //    weixinDbContext.GenericTypeArguments[1],
                //    weixinDbContext.GenericTypeArguments[2]);
            }
            services.TryAddScoped(typeof(ISubscriberStore<>).MakeGenericType(subscriberType), subscriberStoreType);
        }
        else
        {
            //var keyType = foundSubscriberType.GenericTypeArguments[0];//ignore it!
            Type subscriberStoreType = null;
            var weixinDbContext = FindGenericBaseType(contextType, typeof(WeixinDbContext<>));
            if (weixinDbContext == null)
            {
                subscriberStoreType = typeof(SubscriberStore<,>).MakeGenericType(subscriberType, contextType);
            }
            else
            {
                subscriberStoreType = typeof(SubscriberStore<,>).MakeGenericType(subscriberType, contextType);
                //subscriberStoreType = typeof(SubscriberStore<,>).MakeGenericType(subscriberType, contextType,
                //    weixinDbContext.GenericTypeArguments[1],
                //    weixinDbContext.GenericTypeArguments[2]);
            }
            services.TryAddScoped(typeof(ISubscriberStore<>).MakeGenericType(subscriberType), subscriberStoreType);
            services.TryAddScoped(typeof(ISubscriberStore), subscriberStoreType);
        }

        return services;
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
