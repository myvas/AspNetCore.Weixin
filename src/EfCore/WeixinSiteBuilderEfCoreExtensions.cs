using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.EfCore;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
/// </summary>
public static class WeixinSiteBuilderEfCoreExtensions
{
    public static WeixinSiteBuilder AddWeixinEfCore<TWeixinDbContext>(this WeixinSiteBuilder builder, Action<WeixinSiteEfCoreOptions> setupAction = null)
        where TWeixinDbContext : DbContext
    {
        if (setupAction != null)
        {
            builder.Services.Configure(setupAction);
        }
        AddWeixinEfCore(builder.Services, typeof(TWeixinDbContext), null, null);
        return builder;
    }

    public static WeixinSiteBuilder AddWeixinEfCore<TWeixinDbContext, TWeixinSubscriberEntity>(this WeixinSiteBuilder builder, Action<WeixinSiteEfCoreOptions> setupAction = null)
        where TWeixinDbContext : DbContext
        where TWeixinSubscriberEntity : class, IWeixinSubscriberEntity<string>, IEntity, new()
    {
        if (setupAction != null)
        {
            builder.Services.Configure(setupAction);
        }
        AddWeixinEfCore(builder.Services, typeof(TWeixinDbContext), typeof(TWeixinSubscriberEntity), null);
        return builder;
    }

    public static WeixinSiteBuilder AddWeixinEfCore<TWeixinDbContext, TWeixinSubscriberEntity, TKey>(this WeixinSiteBuilder builder, Action<WeixinSiteEfCoreOptions> setupAction = null)
        where TWeixinDbContext : DbContext
        where TWeixinSubscriberEntity : class, IWeixinSubscriberEntity<TKey>, IEntity, new()
        where TKey : IEquatable<TKey>
    {
        if (setupAction != null)
        {
            builder.Services.Configure(setupAction);
        }
        AddWeixinEfCore(builder.Services, typeof(TWeixinDbContext), typeof(TWeixinSubscriberEntity), typeof(TKey));
        return builder;
    }

    /// <summary>
    /// Inject 5 store types, and 1 hosted services.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="contextType"></param>
    /// <param name="subscriberType"></param>
    /// <param name="keyType"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    private static void AddWeixinEfCore(IServiceCollection services, Type contextType, Type subscriberType, Type keyType)
    {
        Type receivedMessageType = typeof(WeixinReceivedMessageEntity);
        Type receivedEventType = typeof(WeixinReceivedEventEntity);
        Type responseMessageType = typeof(WeixinResponseMessageEntity);
        Type sendMessageType = typeof(WeixinSendMessageEntity);

        if (contextType == null)
        {
            throw new ArgumentNullException(nameof(contextType));
        }
        var tryDbContextType = FindBaseType(contextType, typeof(DbContext));
        if (tryDbContextType == null)
        {
            throw new ArgumentException($"{nameof(contextType)} must be derived from DbContext.", nameof(contextType));
        }

        if (subscriberType == null)
        {
            var tryWeixinDbContext2Type = FindBaseInterface(contextType, typeof(IWeixinDbContext<,>));
            if (tryWeixinDbContext2Type != null)
            {
                // IWeixinDbContext<TWeixinSubscriber, TKey>
                keyType ??= tryWeixinDbContext2Type.GenericTypeArguments[1];
                subscriberType = tryWeixinDbContext2Type.GenericTypeArguments[0];
            }
            else
            {
                // IWeixinDbContext<TWeixinSubscriber>
                var tryWeixinDbContext1Type = FindBaseInterface(contextType, typeof(IWeixinDbContext<>));
                if (tryWeixinDbContext1Type != null)
                {
                    subscriberType = tryWeixinDbContext1Type.GenericTypeArguments[0];
                }
                else
                {
                    // If cannot find subscriberType, then the subscriberType must not be null.
                    throw new ArgumentNullException(nameof(subscriberType));
                }
            }
        }

        if (subscriberType != null && keyType == null)
        {
            var trySubscriberType1Type = FindBaseInterface(subscriberType, typeof(IWeixinSubscriberEntity<>));
            if (trySubscriberType1Type != null)
            {
                // IWeixinSubscriber<TKey>
                keyType = trySubscriberType1Type.GenericTypeArguments[0];
            }
            else
            {
                var trySubscriber0Type = FindBaseInterface(subscriberType, typeof(IWeixinSubscriberEntity));
                if (trySubscriber0Type != null)
                {
                    keyType = typeof(string);
                }
                else
                {
                    // If cannot find keyType, then the keyType must not be null.
                    throw new ArgumentNullException(nameof(keyType));
                }
            }
        }

        var subscriberStoreType = typeof(WeixinSubscriberStore<,,>).MakeGenericType(subscriberType, keyType, contextType);
        var receivedMessageStoreType = typeof(WeixinReceivedMessageStore<,>).MakeGenericType(receivedMessageType, contextType);
        var receivedEventStoreType = typeof(WeixinReceivedEventStore<,>).MakeGenericType(receivedEventType, contextType);
        var responseMessageStoreType = typeof(WeixinResponseMessageStore<,>).MakeGenericType(responseMessageType, contextType);
        var sendMessageStoreType = typeof(WeixinSendMessageStore<,>).MakeGenericType(sendMessageType, contextType);

        services.TryAddScoped(typeof(IWeixinSubscriberStore<,>).MakeGenericType(subscriberType, keyType), subscriberStoreType);
        services.TryAddScoped(typeof(IWeixinReceivedMessageStore<>).MakeGenericType(receivedMessageType), receivedMessageStoreType);
        services.TryAddScoped(typeof(IWeixinReceivedEventStore<>).MakeGenericType(receivedEventType), receivedEventStoreType);
        services.TryAddScoped(typeof(IWeixinResponseMessageStore<>).MakeGenericType(responseMessageType), responseMessageStoreType);
        services.TryAddScoped(typeof(IWeixinSendMessageStore<>).MakeGenericType(sendMessageType), sendMessageStoreType);
        if (keyType == typeof(string))
        {
            services.TryAddScoped(typeof(IWeixinSubscriberStore<>).MakeGenericType(subscriberType), typeof(WeixinSubscriberStore<,>).MakeGenericType(subscriberType, contextType));
            if (subscriberType == typeof(WeixinSubscriberEntity))
            {
                services.TryAddScoped(typeof(IWeixinSubscriberStore), typeof(WeixinSubscriberStore<>).MakeGenericType(contextType));
            }
        }
        if (receivedMessageType == typeof(WeixinReceivedMessageEntity))
        {
            services.TryAddScoped(typeof(IWeixinReceivedMessageStore), typeof(WeixinReceivedMessageStore<>).MakeGenericType(contextType));
        }
        if (receivedEventType == typeof(WeixinReceivedEventEntity))
        {
            services.TryAddScoped(typeof(IWeixinReceivedEventStore), typeof(WeixinReceivedEventStore<>).MakeGenericType(contextType));
        }
        if (responseMessageType == typeof(WeixinResponseMessageEntity))
        {
            services.TryAddScoped(typeof(IWeixinResponseMessageStore), typeof(WeixinResponseMessageStore<>).MakeGenericType(contextType));
        }
        if (sendMessageType == typeof(WeixinSendMessageEntity))
        {
            services.TryAddScoped(typeof(IWeixinSendMessageStore), typeof(WeixinSendMessageStore<>).MakeGenericType(contextType));
        }

        // Add event sink
        services.TryAddScoped(typeof(IWeixinEventSink), typeof(WeixinEfCoreEventSink<,>).MakeGenericType(subscriberType, keyType));

        // Add hosted service
        services.TryAddScoped(typeof(DbContextFactory<>).MakeGenericType(contextType));
        services.TryAddScoped(typeof(IWeixinSubscriberSyncService), typeof(WeixinSubscriberSyncService<,,>).MakeGenericType(contextType, subscriberType, keyType));
        services.AddHostedService<WeixinSubscriberSyncHostedService>();
    }

    public static WeixinSiteBuilder AddWeixinEventSink<TWeixinEventSink>(this WeixinSiteBuilder builder)
        where TWeixinEventSink : class, IWeixinEventSink
    {
        builder.Services.Replace(ServiceDescriptor.Scoped<IWeixinEventSink, TWeixinEventSink>());
        return builder;
    }

    /// <summary>
    /// Finds a specific base type (generic or non-generic) in the inheritance hierarchy of a given type.
    /// </summary>
    /// <param name="currentType">The type to inspect.</param>
    /// <param name="baseTypeToFind">The base type to search for (generic or non-generic).</param>
    /// <returns>A TypeInfo object representing the matched type, or null if not found.</returns>
    public static TypeInfo FindBaseType(Type currentType, Type baseTypeToFind)
    {
        if (currentType == null)
        {
            throw new ArgumentNullException(nameof(currentType));
        }

        if (baseTypeToFind == null)
        {
            throw new ArgumentNullException(nameof(baseTypeToFind));
        }

        // Traverse the type and its base types
        Type typeToInspect = currentType;

        while (typeToInspect != null)
        {
            // Check if the current type matches the target type
            if (baseTypeToFind.IsGenericTypeDefinition)
            {
                // Handle generic type definition (e.g., BaseClass<> or IBaseInterface<>)
                if (typeToInspect.IsGenericType && typeToInspect.GetGenericTypeDefinition() == baseTypeToFind)
                {
                    return typeToInspect.GetTypeInfo();
                }
            }
            else
            {
                // Handle non-generic type or interface
                if (typeToInspect == baseTypeToFind)
                {
                    return typeToInspect.GetTypeInfo();
                }
            }

            // Move up the inheritance hierarchy
            typeToInspect = typeToInspect.BaseType;
        }

        // No matching type or interface found
        return null;
    }


    /// <summary>
    /// Finds a specific base interface, generic type, or non-generic type in the inheritance hierarchy of a given type.
    /// </summary>
    /// <param name="currentType">The type to inspect.</param>
    /// <param name="baseInterfaceToFind">The base interface, generic type, or non-generic type to search for.</param>
    /// <returns>A TypeInfo object representing the matched type, or null if not found.</returns>
    public static TypeInfo FindBaseInterface(Type currentType, Type baseInterfaceToFind)
    {
        if (currentType == null)
        {
            throw new ArgumentNullException(nameof(currentType));
        }

        if (baseInterfaceToFind == null)
        {
            throw new ArgumentNullException(nameof(baseInterfaceToFind));
        }

        if (!baseInterfaceToFind.IsInterface)
        {
            throw new ArgumentException("The baseInterfaceTypeToFind parameter must be an interface type.", nameof(baseInterfaceToFind));
        }

        // Traverse the type and its base types
        Type typeToInspect = currentType;

        while (typeToInspect != null)
        {
            // Check all interfaces implemented by the current type
            foreach (Type interfaceType in typeToInspect.GetInterfaces())
            {
                if (baseInterfaceToFind.IsGenericTypeDefinition)
                {
                    // Handle generic interface definition (e.g., IBaseInterface<>)
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == baseInterfaceToFind)
                    {
                        return interfaceType.GetTypeInfo();
                    }
                }
                else
                {
                    // Handle non-generic interface
                    if (interfaceType == baseInterfaceToFind)
                    {
                        return interfaceType.GetTypeInfo();
                    }
                }
            }

            // Move up the inheritance hierarchy
            typeToInspect = typeToInspect.BaseType;
        }

        // No matching type or interface found
        return null;
    }
}
