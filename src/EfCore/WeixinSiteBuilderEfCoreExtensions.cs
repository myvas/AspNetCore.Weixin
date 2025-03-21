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
        where TWeixinSubscriberEntity : class, IWeixinSubscriber<string>, IEntity, new()
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
        where TWeixinSubscriberEntity : class, IWeixinSubscriber<TKey>, IEntity, new()
        where TKey : IEquatable<TKey>
    {
        if (setupAction != null)
        {
            builder.Services.Configure(setupAction);
        }
        AddWeixinEfCore(builder.Services, typeof(TWeixinDbContext), typeof(TWeixinSubscriberEntity), typeof(TKey));
        return builder;
    }

    private static void AddWeixinEfCore(IServiceCollection services, Type contextType, Type subscriberType, Type keyType)
    {
        Type subscriberStoreType = null;
        Type receivedMessageStoreType = null;
        Type receivedEventStoreType = null;
        Type responseMessageStoreType = null;
        Type sendMessageStoreType = null;

        var tryWeixinDbContext2Type = FindBaseInterface(contextType, typeof(IWeixinDbContext<,>));
        if (tryWeixinDbContext2Type != null)
        {
            // IWeixinDbContext<TWeixinSubscriber, TKey>
            keyType = tryWeixinDbContext2Type.GenericTypeArguments[1];
            subscriberType = tryWeixinDbContext2Type.GenericTypeArguments[0];
            services.TryAddScoped(typeof(IWeixinDbContext<,>).MakeGenericType(subscriberType, keyType), contextType);
        }
        else
        {
            // IWeixinDbContext<TWeixinSubscriber>
            var tryWeixinDbContext1Type = FindBaseInterface(contextType, typeof(IWeixinDbContext<>));
            if (tryWeixinDbContext1Type != null)
            {
                subscriberType = tryWeixinDbContext1Type.GenericTypeArguments[0];
                services.TryAddScoped(typeof(IWeixinDbContext<>).MakeGenericType(subscriberType), contextType);
            }
            else
            {
                services.TryAddScoped(typeof(IWeixinDbContext), contextType);
            }

            var trySubscriberType1Type = FindBaseInterface(subscriberType, typeof(IWeixinSubscriber<>));
            if (trySubscriberType1Type != null)
            {
                // IWeixinSubscriber<TKey>
                keyType = trySubscriberType1Type.GenericTypeArguments[0];
            }
            else
            {
                // Set default key type
                keyType = typeof(string);
            }
        }

        subscriberStoreType = typeof(WeixinSubscriberStore<,,>).MakeGenericType(subscriberType, keyType, contextType);
        receivedMessageStoreType = typeof(WeixinReceivedMessageStore<,>).MakeGenericType(typeof(WeixinReceivedMessageEntity), contextType);
        receivedEventStoreType = typeof(WeixinReceivedEventStore<,>).MakeGenericType(typeof(WeixinReceivedEventEntity), contextType);
        responseMessageStoreType = typeof(WeixinResponseMessageStore<,>).MakeGenericType(typeof(WeixinResponseMessageEntity), contextType);
        sendMessageStoreType = typeof(WeixinSendMessageStore<,>).MakeGenericType(typeof(WeixinSendMessageEntity), contextType);

        services.TryAddScoped(typeof(IWeixinSubscriberStore<,>).MakeGenericType(subscriberType, keyType), subscriberStoreType);
        services.TryAddScoped(typeof(IWeixinReceivedMessageStore<>).MakeGenericType(typeof(WeixinReceivedMessageEntity)), receivedMessageStoreType);
        services.TryAddScoped(typeof(IWeixinReceivedEventStore<>).MakeGenericType(typeof(WeixinReceivedEventEntity)), receivedEventStoreType);
        services.TryAddScoped(typeof(IWeixinResponseMessageStore<>).MakeGenericType(typeof(WeixinResponseMessageEntity)), responseMessageStoreType);
        services.TryAddScoped(typeof(IWeixinSendMessageStore<>).MakeGenericType(typeof(WeixinSendMessageEntity)), sendMessageStoreType);
        if (keyType == typeof(string))
        {
            services.TryAddScoped(typeof(IWeixinSubscriberStore<>).MakeGenericType(subscriberType), subscriberStoreType);
            if (subscriberType == typeof(WeixinSubscriberEntity))
            {
                services.TryAddScoped(typeof(IWeixinSubscriberStore), subscriberStoreType);
            }
        }
        services.TryAddScoped(typeof(IWeixinReceivedMessageStore), receivedMessageStoreType);
        services.TryAddScoped(typeof(IWeixinReceivedEventStore), receivedEventStoreType);
        services.TryAddScoped(typeof(IWeixinResponseMessageStore), responseMessageStoreType);
        services.TryAddScoped(typeof(IWeixinSendMessageStore), sendMessageStoreType);

        // Add event sink
        services.Where(x => x.ServiceType == typeof(IWeixinEventSink)).ToList()
            .ForEach(x => services.Remove(x));
        var eventSinkImplType = typeof(WeixinEfCoreEventSink<,>).MakeGenericType(subscriberType, keyType);
        services.TryAddTransient(typeof(IWeixinEventSink), eventSinkImplType);

        // Add hosted service
        services.TryAddScoped(typeof(DbContextFactory<>).MakeGenericType(contextType));
        services.TryAddScoped(typeof(IWeixinSubscriberSyncService), typeof(WeixinSubscriberSyncService<,,>).MakeGenericType(contextType, subscriberType, keyType));
        services.AddHostedService<WeixinSubscriberSyncHostedService>();
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
